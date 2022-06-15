using FarmasiCase.Service.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service.Contracts
{
    public interface IAccountService
    {
        Task<string> Login(AccountLoginDto accountLoginDto);
        Task Logout();
    }
}
