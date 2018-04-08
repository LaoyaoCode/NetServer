using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer
{
    public static class SCThings
    {
        /// <summary>
        /// SQL数据库文件名字，没有分隔符
        /// </summary>
        public const string SQLDirName = "SQLData";
        /// <summary>
        /// 用户信息SQL文件路径
        /// </summary>
        public const string UserInformationSQLFP = SQLDirName + "//" + "UI.s3db";

    }
}
