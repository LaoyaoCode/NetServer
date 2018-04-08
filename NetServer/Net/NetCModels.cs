using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer.Net
{
    /// <summary>
    /// 登录信息模型
    /// </summary>
    [Serializable]
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password;
    }
}
