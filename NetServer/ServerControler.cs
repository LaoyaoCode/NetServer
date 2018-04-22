using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Normal.Net;
using Normal.SQL;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using NetServer.Controler;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetServer
{
    public class ServerControler
    {
        /// <summary>
        /// 外部连接最大等待数
        /// </summary>
        private const int BACKLOG = 25;
        /// <summary>
        /// 服务器监听器
        /// </summary>
        private Socket ServerListener = null;
        /// <summary>
        /// 所有活着的的玩家
        /// </summary>
        public static List<Player> TotalAlivePlayer = new List<Player>();
        /// <summary>
        /// 是否监听端口
        /// </summary>
        private bool IsListening = false;
        private Object ILLock = new object();

        public ServerControler()
        {
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

                Player newPlayer = new Player(connect);

                //开始接受信息
                //ThreadPool.QueueUserWorkItem(this.RecieveDataFromPlayer , connect);
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
