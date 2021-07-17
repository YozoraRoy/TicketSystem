using Dapper;
using DataAccess.Interface;
using DataAccess.Model;
using Platform.IOC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DataAccess.ExecClass
{
    [RegisterIOC]
    public class AccountProvider : IAccountProvider
    {
         private readonly BaseInfoProvider _baseInfoProvider;

        public AccountProvider(BaseInfoProvider baseInfoProvider)
        {
            _baseInfoProvider = baseInfoProvider ?? throw new ArgumentNullException(nameof(IBaseInfoProvider));
        }

        /// <summary>
        /// 建立 Account資料
        /// </summary>
        /// <param name="id">AccountId</param>
        /// <returns></returns>
        public async Task<int> CreatAccount(Account createData)
        {
            int result = -1;

            try
            {
                 result = await _baseInfoProvider.InsertAsync(createData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 查詢 Account資料
        /// </summary>
        /// <param name="accountId">帳號Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<Account>> QueryAccount(int? accountId)
        {
            try
            {
                string sql = @" SELECT * FROM Account ";
                if (accountId.HasValue)
                {
                    sql += @" WHERE AccountId = @AccountId ";
                }
                var param = new DynamicParameters();
                param.Add("AccountId", accountId);

                var result = await _baseInfoProvider.QueryAsync<Account>(sql, param).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查詢 Account資料
        /// </summary>
        /// <param name="email">email</param>
        /// <returns></returns>
        public async Task<IEnumerable<Account>> QueryAccount(string email)
        {
            try
            {
                string sql = @" SELECT * FROM Account ";
                if (string.IsNullOrEmpty(email))
                {
                    sql += @" WHERE Email = @email ";
                }
                var param = new DynamicParameters();
                param.Add("email", email);

                var result = await _baseInfoProvider.QueryAsync<Account>(sql, param).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查詢 Account資料
        /// </summary>
        /// <param name="loginName">登入帳號</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        public async Task<IEnumerable<Account>> QueryAccount(string loginName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
                {
                    return null;
                }

                string sql = @" SELECT * FROM Account ";

                sql += @" WHERE LoginName = @loginName ";

                var param = new DynamicParameters();
                param.Add("loginName", loginName);
                param.Add("passord", password);

                var result = await _baseInfoProvider.QueryAsync<Account>(sql, param).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新 Account資料
        /// </summary>
        /// <param name="id">AccountId</param>
        /// <returns></returns>
        public Task<IEnumerable<Account>> UpdateAccount(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
