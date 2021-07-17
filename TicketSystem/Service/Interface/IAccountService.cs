using TicketSystem.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Service.Interface
{
    public interface IAccountService
    {

         Task<ServiceResult> QueryAccount(string loginName , string password);


    }
}
