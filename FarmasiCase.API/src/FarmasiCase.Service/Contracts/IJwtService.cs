using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service.Contracts
{
    public interface IJwtService
    {
        Task<string> GenerateJwt(string id);
        Task<JwtSecurityToken> Verify(string jwt);
    }
}
