using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using NetServer.Net;
using NetServer.SQL;

namespace NetServer
{
    class Program
    {
        public static ServerControler SC = null;

        public static void Main(string[] args)
        {
            
        }

        public static void Init()
        {
            SC = new ServerControler();
        }
    }
}
