using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{

    public interface IAccountProvider
    {
        /// <summary>
        /// 查詢 Account資料
        /// </summary>
        /// <param name="accountId">帳號Id</param>
        /// <returns></returns>
        Task<IEnumerable<Account>> QueryAccount(int? id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<IEnumerable<Account>> QueryAccount(string email);

        /// <summary>
        /// 查詢 Account資料
        /// </summary>
        /// <param name="loginName">帳號名稱</param>
        /// <param name="passord">密碼</param>
        /// <returns></returns>
        Task<IEnumerable<Account>> QueryAccount(string loginName, string passord);

        /// <summary>
        /// 建立 Account資料
        /// </summary>
        /// <param name="createData">createData</param>
        /// <returns></returns>
        Task<int> CreatAccount(Account createData);

        /// <summary>
        /// 更新 Account資料
        /// </summary>
        /// <param name="id">AccountId</param>
        /// <returns></returns>
        Task<IEnumerable<Account>> UpdateAccount(int? id);

    }
}
