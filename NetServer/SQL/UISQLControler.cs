using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.IO;
using NetServer;

namespace NetServer.SQL
{
    public class UISQLControler
    {
        /// <summary>
        /// 表名
        /// </summary>
        public const String TableName = "User";

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static String ConnectingString =
            "Data Source =" + SCThings.UserInformationSQLFP + ";" + "Version=3";

        /// <summary>
        /// 检查创建字符串
        /// </summary>
        private static String CreateRecordTableSqlStatement =
            "CREATE TABLE IF NOT EXISTS User"
            + "("
            + "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
            + "UserName VARCHAR(15) NOT NULL,"
            + "Password VARCHAR(25) NOT NULL,"
            + "TotalMoney INTEGER NOT NULL,"
            + "HadPlayerModelsID TEXT NOT NULL,"
            + "MFriendsID TEXT NOT NULL,"
            + "Name VARCHAR(15) NOT NULL"
            + ");";

        /// <summary>
        /// 唯一实例
        /// </summary>
        public static UISQLControler UnityIns = new UISQLControler();


        public UISQLControler()
        {
            CheckAndInit();
        }
        private void CheckAndInit()
        {
            //检查数据文件夹是否存在
            DirectoryInfo datasDir = new DirectoryInfo(SCThings.SQLDirName);
            //不存在则创建
            if (!datasDir.Exists)
            {
                datasDir.Create();
            }

            //检查数据库是否存在，如不存在，则直接创建并且初始化
            using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
            {
                sqlConnect.Open();

                SQLiteCommand command = sqlConnect.CreateCommand();
                command.CommandText = CreateRecordTableSqlStatement;

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 添加一个用户记录
        /// test success , 2018.4.9
        /// </summary>
        /// <param name="model">用户模型数据</param>
        /// <returns></returns>
        public bool AddUser(UserInformatioonModel model)
        {
            bool isSucceeded = false;
            StringBuilder builder = new StringBuilder();

            try
            {
                //连接数据库
                using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
                {
                    sqlConnect.Open();

                    builder.Append("INSERT INTO ");
                    builder.Append(TableName);
                    builder.Append("(UserName,Password,TotalMoney,HadPlayerModelsID,MFriendsID,Name)");
                    builder.Append(" ");

                    builder.Append("VALUES");
                    builder.Append("(");
                    builder.Append("\'" + model.UserName + "\',");
                    builder.Append("\'" + model.Password + "\',");
                    builder.Append(model.TotalMoney + ",");
                    builder.Append("\'" + model.HadPlayerModelsID + "\',");
                    builder.Append("\'" + model.MFriendsID + "\',");
                    builder.Append("\'" + model.Name + "\'");
                    builder.Append(");");

                    SQLiteCommand command = sqlConnect.CreateCommand();
                    command.CommandText = builder.ToString();

                    //成功添加
                    if (command.ExecuteNonQuery() == 1)
                    {
                        isSucceeded = true;

                        String getIdCommandString = "SELECT last_insert_rowid() FROM " + TableName;

                        SQLiteCommand getIdCommand = sqlConnect.CreateCommand();
                        getIdCommand.CommandText = getIdCommandString;

                        //读取刚刚插入的ID，赋值给模型
                        using (SQLiteDataReader reader = getIdCommand.ExecuteReader())
                        {
                            reader.Read();
                            model.ID = reader.GetInt32(0);
                        }
                    }
                }

                return isSucceeded;
            }
            catch(Exception e)
            {
                return false;
            }
            
        }

        /// <summary>
        /// 更新一个用户数据
        /// test success , 2018.4.9
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpDateUser(UserInformatioonModel model)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                //连接数据库
                using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
                {
                    sqlConnect.Open();

                    builder.Append("UPDATE ");
                    builder.Append(TableName);
                    builder.Append(" ");

                    builder.Append("SET ");
                    builder.Append("UserName=" + "\'" + model.UserName + "\',");
                    builder.Append("Password=" + "\'" + model.Password + "\',");
                    builder.Append("TotalMoney=" + model.TotalMoney + ",");
                    builder.Append("HadPlayerModelsID=" + "\'" + model.HadPlayerModelsID + "\',");
                    builder.Append("MFriendsID=" + "\'" + model.MFriendsID + "\',");
                    builder.Append("Name=" + "\'" + model.Name + "\'");
                    builder.Append(" ");

                    builder.Append("WHERE ID=" + model.ID + ";");

                    SQLiteCommand command = sqlConnect.CreateCommand();
                    command.CommandText = builder.ToString();

                    //成功更新
                    if (command.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一个记录
        /// test success , 2018.4.9
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(int id)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                //连接数据库
                using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
                {
                    sqlConnect.Open();

                    builder.Append("DELETE FROM ");
                    builder.Append(TableName);
                    builder.Append(" ");

                    builder.Append("WHERE ID=");
                    builder.Append(id);
                    builder.Append(";");

                    SQLiteCommand command = sqlConnect.CreateCommand();
                    command.CommandText = builder.ToString();

                    //成功删除
                    if (command.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }    
            }
            catch(Exception e)
            {
                return false;
            }
            
        }

        /// <summary>
        /// 获取所有的用户信息
        /// test success , 2018.4.9
        /// </summary>
        /// <returns></returns>
        public List<UserInformatioonModel> GetAllUser()
        {
            List<UserInformatioonModel> result = new List<UserInformatioonModel>();

            try
            {
                //连接数据库
                using (SQLiteConnection sqlConnect = new SQLiteConnection(ConnectingString))
                {
                    sqlConnect.Open();

                    String commandString = "SELECT * FROM " + TableName;
                    SQLiteCommand command = sqlConnect.CreateCommand();
                    command.CommandText = commandString;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserInformatioonModel user = new UserInformatioonModel();

                            user.ID = int.Parse(reader["ID"].ToString());
                            user.UserName = reader["UserName"].ToString();
                            user.Password = reader["Password"].ToString();
                            user.TotalMoney = int.Parse(reader["TotalMoney"].ToString());
                            user.HadPlayerModelsID = reader["HadPlayerModelsID"].ToString();
                            user.MFriendsID = reader["MFriendsID"].ToString();
                            user.Name = reader["Name"].ToString();

                            result.Add(user);
                        }

                        return result;
                    }
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
