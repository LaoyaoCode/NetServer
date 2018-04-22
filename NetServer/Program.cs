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
        private const string ALine = "----------------------------------------------------------";
        private const string AddUserCommand = "AUser";

        public static void Main(string[] args)
        {
            string cmd = null;

            Init();

            Console.WriteLine(ALine);
            Console.WriteLine("开始监听玩家的链接");
            Console.WriteLine(ALine);

            while (true)
            {
                Console.Write(">> ");

                cmd = Console.ReadLine();

                if (cmd == AddUserCommand)
                {
                    AddUser();
                }

            }
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
            return true;
        }

        //添加一个用户
        public static void AddUser()
        {
            string userName, passWords,name;

            Console.WriteLine(ALine);

            Console.Write("UserName : ");
            userName = Console.ReadLine();

            while(true)
            {
                if(userName.Length > 10)
                {
                    Console.WriteLine("Error : 用户名长度不得长于10个字符");
                    Console.Write("UserName : ");
                    userName = Console.ReadLine();
                }
                else if(UISQLControler.UnityIns.GetUserFromUserName(userName) != null)
                {
                    Console.WriteLine("Error : 用户名已存在");
                    Console.Write("UserName : ");
                    userName = Console.ReadLine();
                }
                else
                {
                    break;
                }  
            }

            Console.Write("Password : ");
            passWords = Console.ReadLine();
            while(true)
            {
                if(passWords.Length > 20)
                {
                    Console.WriteLine("Error : 密码长度不得长于20个字符");
                    Console.Write("Password : ");
                    passWords = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            Console.Write("Name : ");
            name = Console.ReadLine();
            while (true)
            {
                if (name.Length > 10)
                {
                    Console.WriteLine("Error : 名字不得长于10个字符");
                    Console.Write("Name : ");
                    name = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            UserInformationModel ui = new UserInformationModel();
            ui.UserName = userName;
            ui.Password = passWords;
            ui.Name = name;
            ui.TotalMoney = 0;

            if(UISQLControler.UnityIns.AddUser(ui))
            {
                Console.WriteLine("添加用户成功");
            }
            else
            {
                Console.WriteLine("添加用户失败");
            }

            Console.WriteLine(ALine);
        }
    }
}
