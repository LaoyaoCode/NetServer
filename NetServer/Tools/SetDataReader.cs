using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetServer.Tools
{
    public class SetDataReader
    {
        /// <summary>
        /// 最大线程数
        /// </summary>
        public readonly int MaxThreadN;
        /// <summary>
        /// 最小线程数
        /// </summary>
        public readonly int MinThreadN;
        /// <summary>
        /// 端口数
        /// </summary>
        public readonly int PortN;

        /// <summary>
        /// 根节点名字
        /// </summary>
        private const string RootName = "AS";
        /// <summary>
        /// 各个数据节点名字
        /// </summary>
        private const string MaxTNName = "MaxThreadNumber";
        private const string MinTNName = "MinThreadNumber";
        private const string PortNName = "PortNumber";

        public SetDataReader()
        {
           
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode rootNode = null;

                //获取XML文档
                doc.Load(SCThings.AppSetDataPath);
                //获取根节点
                rootNode = doc.SelectSingleNode(RootName);

                //获取数据值
                MaxThreadN = int.Parse(rootNode.SelectSingleNode(MaxTNName).InnerText);
                MinThreadN = int.Parse(rootNode.SelectSingleNode(MinTNName).InnerText);
                PortN = int.Parse(rootNode.SelectSingleNode(PortNName).InnerText);
            }
            catch(Exception e)
            {
                throw new Exception("程序初始化数据读取失败");
            }
        }
    }
}
