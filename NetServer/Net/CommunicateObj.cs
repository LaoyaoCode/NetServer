using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Normal.Net
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
            Login,
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
        public String Data = null;
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

        public CommunicateObj()
        {

        }

        /// <summary>
        /// 创建网络交流传输对象
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="data">传输数据</param>
        /// <param name="isSuccess">是否成功，服务端发送给客户端</param>
        /// <param name="detail">要发送给客户端的细节字符串</param>
        public CommunicateObj(DataTypeEnum dataType, Object data, bool isSuccess, string detail, Type type = null)
        {
            StringBuilder buffer = new StringBuilder();
            DataType = dataType;
            IsSuccess = isSuccess;
            DetailString = detail;

            if ((data != null) &&(type == null))
            {
                throw new Exception("没有标明数据类型");
            }

            

            if(data != null)
            {
                //序列化数据为字符串
                XmlSerializer serializer = new XmlSerializer(type);
                using (TextWriter writer = new StringWriter(buffer))
                {
                    serializer.Serialize(writer, data);
                }
                Data = buffer.ToString();
            }
            
        }

        /// <summary>
        /// 创建网络交流传输对象
        /// </summary>
        /// <param name="dataType">传输对象类型</param>
        /// <param name="data">传输数据</param>
        public CommunicateObj(DataTypeEnum dataType, Object data, Type type = null)
        {
            StringBuilder buffer = new StringBuilder();
            DataType = dataType;

            if ((data != null) && (type == null))
            {
                throw new Exception("没有标明数据类型");
            }

            if(data != null)
            {
                //序列化数据为字符串
                XmlSerializer serializer = new XmlSerializer(type);
                using (TextWriter writer = new StringWriter(buffer))
                {
                    serializer.Serialize(writer, data);
                }

                Data = buffer.ToString();
            }
            
        }

        /// <summary>
        /// 创建网络交流传输对象
        /// </summary>
        /// <param name="dataType">传输对象类型</param>
        /// <param name="data">传输数据</param>
        public CommunicateObj(DataTypeEnum dataType, Object data,bool isSuccess ,  Type type = null)
        {
            StringBuilder buffer = new StringBuilder();
            DataType = dataType;
            IsSuccess = isSuccess;

            if ((data != null) && (type == null))
            {
                throw new Exception("没有标明数据类型");
            }

            if (data != null)
            {
                //序列化数据为字符串
                XmlSerializer serializer = new XmlSerializer(type);
                using (TextWriter writer = new StringWriter(buffer))
                {
                    serializer.Serialize(writer, data);
                }

                Data = buffer.ToString();
            }

        }

        /// <summary>
        /// 获得数据对象，前提是Data != string.Empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDataObj<T>()
        {
            T cloneObject = default(T);

            StringBuilder buffer = new StringBuilder();

            buffer.Append(Data);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(buffer.ToString()))
            {
                Object obj = serializer.Deserialize(reader);
                cloneObject = (T)obj;
            }

            return cloneObject;
        }
    }
}