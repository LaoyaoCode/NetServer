using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetServer.SQL;
using System.Net;
using System.Net.Sockets;

namespace NetServer.Controler
{
    public class Player
    {
        private UserInformationModel UIModel = null;
        private Socket ConnectSocket = null;

        public Player(UserInformationModel uiModel , Socket connect)
        {
            UIModel = uiModel;
            ConnectSocket = connect;
        }

    }
}
