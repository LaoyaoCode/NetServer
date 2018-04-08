using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer.Net
{
    [Serializable]
    public class CommunicateObj
    {
        /// <summary>
        /// 数据类型枚举体
        /// </summary>
        [Serializable]
        public enum DataTypeEnum
        {
            /// <summary>
            /// 用户登录
            /// </summary>
            Login ,
        }

        /// <summary>
        /// 交流数据
        /// </summary>
        public Object Data = null;
        /// <summary>
        /// 是否成功，由服务端返回给客户端的数据才需要
        /// </summary>
        public bool IsSuccess = true;
        /// <summary>
        /// 细节字符串
        /// </summary>
        public string DetialString = string.Empty;
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataTypeEnum DataType;
    }
}
