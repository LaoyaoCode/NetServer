using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetServer.Net;
using NetServer.SQL;
using NetServer.Models ;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using NetServer.Controler;
using System.IO;

namespace NetServer
{
    public class ServerControler
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private const int BUFFSIZE = 1024;
        /// <summary>
        /// 外部连接最大等待数
        /// </summary>
        private const int BACKLOG = 25;
        /// <summary>
        /// 所有用户信息
        /// </summary>
        private List<UserInformationServer> TotalUserInformation = new List<UserInformationServer>();
        /// <summary>
        /// 服务器监听器
        /// </summary>
        private Socket ServerListener = null;
        /// <summary>
        /// 所有活着的的玩家
        /// </summary>
        private List<Player> TotalAlivePlayer = new List<Player>();
        /// <summary>
        /// 是否监听端口
        /// </summary>
        private bool IsListening = false;
        private Object ILLock = new object();

        public ServerControler()
        {
            //获取所有的用户信息
            //将其保存在列表中                                                             
            List<UserInformationModel> models = UISQLControler.UnityIns.GetAllUser();
            for(int counter = 0; counter < models.Count;counter++)
            {
                UserInformationServer uiServer = new UserInformationServer();
                uiServer.UIModel = models[counter];
                TotalUserInformation.Add(uiServer);
            }

            try
            {
                //初始化套接字监听器
                ServerListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ServerListener.Bind(new IPEndPoint(IPAddress.Any, Program.SDReader.PortN));
                ServerListener.Listen(BACKLOG);
            }
            catch(Exception e)
            {
                throw new Exception("网络监听套接字初始化失败");
            }


            IsListening = true;

            ThreadPool.QueueUserWorkItem(this.WaitForRecieveClientConnect);
        }

        private void WaitForRecieveClientConnect(object state)
        {
            while(true)
            {
                lock(ILLock)
                {
                    //如果不需要再监听，则跳出监听循环
                    if(!IsListening)
                    {
                        break;
                    }
                }

                //接受到客户端的链接
                Socket connect = ServerListener.Accept();
                //开始接受信息
                ThreadPool.QueueUserWorkItem(this.RecieveDataFromPlayer , connect);
            }
        }

        /// <summary>
        /// 从玩家那里接受信息
        /// </summary>
        /// <param name="socket"></param>
        private void RecieveDataFromPlayer(object socket)
        {
            Socket connect = (Socket)socket;
            Player player = null;

            while (true)
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

                while (totalHeaderByteNumberRecieve != 2)
                {
                    try
                    {
                        byteRecieveOnce = connect.Receive(header, totalHeaderByteNumberRecieve, 2 - totalHeaderByteNumberRecieve, SocketFlags.None);

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


                if(isConnectClose)
                {
                    Console.WriteLine("IP : " + ((System.Net.IPEndPoint)connect.RemoteEndPoint).Address.ToString() + " 未完成登录断开连接");
                    break;
                }
                else
                {
                    dataLength = (UInt16)(header[0] * 256 + header[1]);
                }


            }
        }

        public void Close()
        {
            lock(ILLock)
            {
                IsListening = false;
            }
            
        }
    }
}
