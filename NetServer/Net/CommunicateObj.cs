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
            /// <summary>
            /// 用户登录返回数据
            /// </summary>
            LoginInBack,
            /// <summary>
            /// 用于标识心跳表，监测client是否还活着
            /// </summary>
            IsAlive
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
        public string DetailString = string.Empty;
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataTypeEnum DataType;

        /// <summary>
        /// 创建网络交流传输对象
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="data">传输数据</param>
        /// <param name="isSuccess">是否成功，服务端发送给客户端</param>
        /// <param name="detail">要发送给客户端的细节字符串</param>
        public CommunicateObj(DataTypeEnum dataType , Object data , bool isSuccess , string detail)
        {
            DataType = dataType;
            Data = data;
            IsSuccess = isSuccess;
            DetailString = detail;
        }

        /// <summary>
        /// 创建网络交流传输对象
        /// </summary>
        /// <param name="dataType">传输对象类型</param>
        /// <param name="data">传输数据</param>
        public CommunicateObj(DataTypeEnum dataType , Object data)
        {
            DataType = dataType;
            Data = data;
        }
    }
}
