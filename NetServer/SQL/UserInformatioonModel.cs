using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetServer.SQL
{
    [Serializable]
    public class UserInformatioonModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 用户名 , 最大 10 位英文
        /// </summary>
        public string UserName = string.Empty;

        /// <summary>   
        /// 密码 ， 最大20 位英文
        /// </summary>
        public string Password = string.Empty;

        /// <summary>
        /// 所有的金钱
        /// </summary>
        public int TotalMoney = 0;

        /// <summary>
        /// 拥有的玩家模型ID,em: 1$2$3$10$11,保存在字符串里面
        /// </summary>
        public string HadPlayerModelsID = string.Empty;

        /// <summary>
        /// 对应朋友的ID，em: 1$2$3%4$11$100
        /// </summary>
        public string MFriendsID = string.Empty;

        /// <summary>
        /// 玩家名字,只能由英文字母组成，最大10位
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// 获取所有拥有的玩家模型ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllPModelsID()
        {
            List<int> result = new List<int>();

            if(HadPlayerModelsID == string.Empty)
            {
                
            }
            else
            {
                string[] temp = HadPlayerModelsID.Split('$');

                for(int counter = 0; counter < temp.Length; counter++)
                {
                    result.Add(int.Parse(temp[counter]));
                }
            }

            return result;
        }

        /// <summary>
        /// 增加拥有的玩家模型ID
        /// </summary>
        /// <param name="id">模型ID</param>
        public void AddAPModelsID(int id)
        {
            if(HadPlayerModelsID == string.Empty)
            {
                HadPlayerModelsID += id;
            }
            else
            {
                HadPlayerModelsID += '$' + id;
            }
        }

        /// <summary>
        /// 获取所有我的朋友ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllMFriendsID()
        {
            List<int> result = new List<int>();

            if(MFriendsID == string.Empty)
            {
                
            }
            else
            {
                string[] temp = MFriendsID.Split('$');
                
                for(int counter = 0; counter < temp.Length; counter++)
                {
                    result.Add(int.Parse(temp[counter]));
                }
            }

            return result;
        }

        public void AddMFriendsID(int id)
        {
            if(MFriendsID == string.Empty)
            {
                MFriendsID += id;
            }
            else
            {
                MFriendsID += "$" + id;
            }
        }
    }
}
