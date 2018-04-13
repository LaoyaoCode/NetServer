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

namespace NetServer
{
    public class ServerControler
    {
        /// <summary>
        /// 外部连接最大等待数
        /// </summary>
        private const int BACKLOG = 25;
        /// <summary>
        /// 所有用户信息
        /// </summary>
        public List<UserInformationServer> TotalUserInformation = new List<UserInformationServer>();
        public Socket ServerListener = null;

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
           

            ThreadPool.QueueUserWorkItem(this.WaitForRecieveClientConnect);
        }

        private void WaitForRecieveClientConnect(object state)
        {
            
        }
    }
}
