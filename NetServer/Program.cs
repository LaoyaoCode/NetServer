using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Normal.Net;
using Normal.SQL;
using NetServer.Tools;

namespace NetServer
{
    class Program
    {
        public static ServerControler SC = null;
        public static SetDataReader SDReader = null ;

        public static void Main(string[] args)
        {
            Init();       
        }

        public static bool Init()
        {
            try
            {
                SDReader = new SetDataReader();
                Console.WriteLine("程序设定数据读取完成");

                //配置线程池
                ThreadPool.SetMaxThreads(SDReader.MaxThreadN, SDReader.MaxThreadN);
                ThreadPool.SetMinThreads(SDReader.MinThreadN, SDReader.MinThreadN);

                SC = new ServerControler();
                Console.WriteLine("服务器控制对象初始化完成");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error : " + e.Message);
                return false;
            }

            while(true)
            {
                Console.ReadLine();
                break;
            }

            return true;
        }
    }
}
