using TicketSystem.Helper;
using TicketSystem.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService _iAccountService;
        private readonly IConfiguration _config;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IAccountService iAccountService, IConfiguration config)
        {
            _iAccountService = iAccountService ?? throw new ArgumentException(nameof(iAccountService));
            _config = config ?? throw new ArgumentException(nameof(config));
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        // [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="loginName">Email帳號</param>
        /// <param name="password">SHA256 Hash - Base64</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ServiceResult>> Login(string loginName, string password, string returnUrl)
        {
            // init
            string claimData = string.Empty;
            string claimRole = string.Empty;
            var result = new ServiceResult();

            try
            {
                if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
                {
                    result = new ServiceResult(false, ServiceResult.FaildOfErrorCode, "請確認帳號或密碼欄位資料，不可以空白");
                    return Ok(result);
                }

                // 管理者帳號 Hard Ccode
                var hashPassword = Sha256encrypt("admin");
                if (loginName == "admin" && password == hashPassword)
                {
                    claimData = "Admin";
                    claimRole = "Admin";
                }
                else
                {
                    var sertviceResult = await _iAccountService.QueryAccount(loginName, password).ConfigureAwait(false);
                    if (!sertviceResult.IsOk)
                    {
                        result = new ServiceResult(false, ServiceResult.FaildOfErrorCode, sertviceResult.Message);
                        return Ok(result);
                    }

                    claimData = loginName;
                    claimRole = "User";
                }

                await ProcUserLogin(claimData, claimRole).ConfigureAwait(false);

                // 防止Open Redirect漏洞
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);//導到原始要求網址
                }
                else
                {
                    result = new ServiceResult(true, ServiceResult.SuccessCode, "登入成功!");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                // TODO Error Log
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        /// <summary>
        /// 處理登入邏輯
        /// </summary>
        /// <param name="claimData">自定義登入資料</param>
        /// <param name="claimRole">登入角色</param>
        /// <returns></returns>
        private async Task ProcUserLogin(string claimData, string claimRole)
        {
            Claim[] claims = new[] {
                    new Claim("LoginInfo", claimData), // 自定義資料 未來可以做特殊判斷
                    new Claim(ClaimTypes.Name, claimData), // TODO 改成存放暱稱
                    new Claim(ClaimTypes.Role, claimRole),
                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); // Scheme 必填
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            // 從組態讀取登入逾時設定
            // double loginExpireMinute = _config.GetValue<double>("LoginExpireMinute");

            await HttpContext.SignInAsync(principal
            // new AuthenticationProperties()
            //{
            //    IsPersistent = false, //IsPersistent = false：瀏覽器關閉立馬登出；IsPersistent = true 就變成常見的Remember Me功能
            //                          //用戶頁面停留太久，逾期時間，在此設定的話會覆蓋Startup.cs裡的逾期設定
            //    /* ExpiresUtc = DateTime.UtcNow.AddMinutes(loginExpireMinute) */
            //}
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// 處理 sha256加密
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        private static string Sha256encrypt(string phrase)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(phrase);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            string result = Convert.ToBase64String(passwordBytes);
            return result;
        }

    }
}
