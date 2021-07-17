using DataAccess.Interface;
using TicketSystem.Helper;
using TicketSystem.Service.Interface;
using Platform.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Service.ExecClass
{
    [RegisterIOC]
    public class AccountService : IAccountService
    {
        private readonly IAccountProvider _iAccountProvider;

        public AccountService(IAccountProvider iAccountProvider)
        {            
            _iAccountProvider = iAccountProvider ?? throw new ArgumentException(nameof(AccountService));
        }

        /// <summary>
        /// 讀取帳號
        /// </summary>
        /// <param name="loginName">登入帳號</param>
        /// <param name="password">登入密碼</param>
        /// <returns></returns>
        public async Task<ServiceResult> QueryAccount(string loginName, string password)
        {
            // 預設找不到
            var result = new ServiceResult(false, "沒有找到帳號");

            // 字串處理.
            loginName = loginName.Trim();
            password = password.Trim();

            // 商業邏輯
            var resultRaw = await _iAccountProvider.QueryAccount(loginName, password).ConfigureAwait(false);
            if (resultRaw.Any())
            {
                return new ServiceResult(true, "有找到帳號");
            }

            return result;
        }
    }
}
