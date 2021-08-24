using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using wizeline.Model;

namespace wizeline.Service
{
    public interface IAccountService
    {
        Task<string> GetUserSalt(string username);
        Task<UserModel> AuthenticateUser(string username, string password, string salt);
        string CreateToken(string signVerify);
    }
}
