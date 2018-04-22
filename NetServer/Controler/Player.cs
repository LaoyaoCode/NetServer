using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Normal.SQL;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NetServer.Controler;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetServer;
using Normal.Net;
using System.Xml.Serialization;

namespace NetServer.Controler
{
    public class Player
    {
        private UserInformationModel UIModel = null;
        private Socket ConnectSocket = null;
        private object SendLock = new object();
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private const int BUFFSIZE = 1024;
        private bool IsConnectClose = false;

        public Player(Socket connect)
        {
            ConnectSocket = connect;

            ThreadPool.QueueUserWorkItem(this.RecieveDataFromPlayer);
        }

        /// <summary>
        /// 从玩家那里接受信息
        /// 绝对只有一个线程在跑这个函数，只能从这个函数接受
        /// 不需要考虑资源占用
        /// </summary>
        /// <param name="socket"></param>
        private void RecieveDataFromPlayer(object state)
        {
            while (!IsConnectClose)
            {
                //数据长度
                UInt16 dataLength = 0;
                //头数据，代表后面数据长度
                byte[] header = new byte[2];
                //一次接受的字节
                int byteRecieveOnce = 0;
                //总共接受的头字节数目
                int totalHeaderByteNumberRecieve = 0;
                //总共接受的数据字节数目
                int totalDataByteNumberRecive = 0;
                //是否链接被关闭
                bool isConnectClose = false;
                //数据数据缓冲区
                MemoryStream ms = null;
                //缓冲区
                byte[] buff = new byte[BUFFSIZE];

                while (totalHeaderByteNumberRecieve < 2)
                {
                    try
                    {
                        byteRecieveOnce = ConnectSocket.Receive(header, totalHeaderByteNumberRecieve, 2 - totalHeaderByteNumberRecieve, SocketFlags.None);

                        //链接已经被关闭了
                        if (byteRecieveOnce == 0)
                        {
                            isConnectClose = true;
                            break;
                        }
                        else
                        {
                            totalHeaderByteNumberRecieve += byteRecieveOnce;
                        }
                    }
                    catch
                    {
                        //链接已经被关闭了
                        isConnectClose = true;
                        break;
                    }
                }


                if (isConnectClose)
                {
                    LostConnect();

                    //如果玩家之前已经登陆了，则在活跃玩家列表里面去除这个玩家
                    if (UIModel != null)
                    {
                        ServerControler.TotalAlivePlayer.Remove(this);
                    }

                    Console.WriteLine("IP : " + ((System.Net.IPEndPoint)ConnectSocket.RemoteEndPoint).Address.ToString() + " 断开连接");
                    break;
                }
                else
                {
                    dataLength = (UInt16)(header[0] * 256 + header[1]);
                }

                while (totalDataByteNumberRecive < dataLength)
                {
                    ms = new MemoryStream();

                    try
                    {
                        byteRecieveOnce = ConnectSocket.Receive(buff, BUFFSIZE, SocketFlags.None);

                        //数据未接受完全之前就已经断开了连接
                        if (byteRecieveOnce == 0)
                        {
                            isConnectClose = true;
                            break;
                        }
                        else
                        {
                            //保存已经接受的数据数目
                            totalDataByteNumberRecive += byteRecieveOnce;
                            //保存已经接受到的数据到内存流中
                            ms.Write(buff, 0, byteRecieveOnce);
                        }
                    }
                    catch
                    {
                        isConnectClose = true;
                        break;
                    }
                }

                if (isConnectClose)
                {
                    LostConnect();

                    if (UIModel != null)
                    {
                        ServerControler.TotalAlivePlayer.Remove(this);
                    }

                    //清除流数据
                    ms.Close();

                    Console.WriteLine("IP : " + ((System.Net.IPEndPoint)ConnectSocket.RemoteEndPoint).Address.ToString() + " 断开连接");
                    break;
                }
                else
                {
                    //重新调整内存流的位置
                    ms.Flush();
                    ms.Position = 0;

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(CommunicateObj));
                    if (ms.Capacity > 0)
                    {
                        try
                        {
                            //反序列化
                            CommunicateObj obj = (CommunicateObj)xmlSerializer.Deserialize(ms);
                            //处理数据
                            DisposeData(obj);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("IP : " + ((System.Net.IPEndPoint)ConnectSocket.RemoteEndPoint).Address.ToString() + " 数据格式错误，反序列化失败");
                        }

                        //清除流数据
                        ms.Close();
                    }
                    else
                    {
                        //清除流数据
                        ms.Close();
                        Console.WriteLine("IP : " + ((System.Net.IPEndPoint)ConnectSocket.RemoteEndPoint).Address.ToString() + " 发送了空数据");
                    }
                }

            }
        }

        /// <summary>
        /// 对着用户发送数据
        /// </summary>
        /// <param name="connect"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SendMessage(CommunicateObj obj)
        {
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CommunicateObj));
            MemoryStream ms = new MemoryStream();
            int onceRead = 0;
            UInt16 length = 0;
            int headerFlag = 2;

            //如果连接已经关闭，则无法发送数据
            if(IsConnectClose)
            {
                ms.Close();
                return false;
            }


            try
            {
                //binaryFormatter.Serialize(ms, obj);
                xmlSerializer.Serialize(ms, obj);
            }
            //序列化错误
            catch
            {
                return false;
            }

            ms.Flush();
            byte[] buff = new byte[BUFFSIZE];
            ms.Position = 0;

            //设置头数据
            length = (UInt16)ms.Length;
            buff[0] = (byte)(length / 256);
            buff[1] = (byte)(length % 256);

            try
            {
                //发送数据
                lock (SendLock)
                {
                    while ((onceRead = ms.Read(buff, headerFlag, BUFFSIZE - headerFlag)) > 0)
                    {
                        ConnectSocket.Send(buff, 0, onceRead + headerFlag, SocketFlags.None);
                        headerFlag = 0;
                    }
                }

                //关闭内存流
                ms.Close();

                return true;
            }
            //发送的时候关闭了连接
            catch
            {
                //关闭内存流
                ms.Close();

                LostConnect();

                if (UIModel != null)
                {
                    ServerControler.TotalAlivePlayer.Remove(this);
                }

                return false;
            }
            

        }


        private void DisposeData(CommunicateObj obj)
        {
            if (UIModel != null)
            {

            }
            else
            {
                //如果是登录信息的话
                if (obj.DataType == CommunicateObj.DataTypeEnum.Login)
                {
                    LoginModel liModel = obj.GetDataObj<LoginModel>();
                    UserInformationModel uiModel = UISQLControler.UnityIns.GetUserFromUserName(liModel.UserName);

                    //如果查找不到用户名
                    //密码错误
                    if ((uiModel == null) || (uiModel.Password != liModel.Password))
                    {
                        CommunicateObj objBack = new CommunicateObj(CommunicateObj.DataTypeEnum.LoginInBack, null, false, "用户名不存在" );
                        this.SendMessage(objBack);
                    }
                    else
                    {
                        UIModel = uiModel;
                        CommunicateObj objBack = new CommunicateObj(CommunicateObj.DataTypeEnum.LoginInBack, uiModel, true , typeof(UserInformationModel));
                        this.SendMessage(objBack);
                    }

                }
            }

        }

        /// <summary>
        /// 用户失去连接时候调用
        /// </summary>
        public void LostConnect()
        {
            //标示为连接关闭
            IsConnectClose = true;
        }
    }
}
