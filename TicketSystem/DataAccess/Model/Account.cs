using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class Account
    {

        public string LoginName { get; set; }

        public string Passord { get; set; }

        public string Email { get; set; }

        // public Guid Guid { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 稱號
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 等級
        /// </summary>
        // public int Lv { get; set; }

        /// <summary>
        /// 角色 0 Administrator 1 PM 2 QA 3 RD
        /// </summary>
        public int Role { get; set; }

    }
}
