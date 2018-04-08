using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetServer.Net;
using NetServer.SQL;

namespace NetServer
{
    public class ServerControler
    {
        public List<UserInformatioonModel> TotalUserInformation = null;

        public ServerControler()
        {
            //获取所有的用户信息
            TotalUserInformation = UISQLControler.UnityIns.GetAllUser();
        }
    }
}
