using Dapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using wizeline.Common;
using wizeline.Model;

namespace wizeline.Service
{
    public class AccountService : IAccountService
    {
        //we can inject this in a configuration object
        private string signVerify = "my2w7wjd7yXF64FIADfJxNs1oupTGAuW";
        public async Task<UserModel> AuthenticateUser(string username, string password, string salt)
        {
            var user = new UserModel();
            user = null;
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection("Server=bootcamp-tht.sre.wize.mx;Database=bootcamp_tht;User Id=secret;Password=noPow3r"))
            {
                string pass = EncryptionFunctions.GetSHA256(salt, password);
                string sqlUser = "SELECT username, role FROM users where username = @username and password = @password";
                user = await connection.QueryFirstOrDefaultAsync<UserModel>(sqlUser, new { username = username, password = pass });
                if(user != null) user.Token = CreateToken(signVerify);
            }

            return user;
        }

        public string CreateToken(string signVerify)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(signVerify);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new[] { new Claim("Role", "admin") })
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var strToken = tokenHandler.WriteToken(token);
            return strToken;
        }

        public async Task<string> GetUserSalt(string username)
        {
            string sqlSalt = "SELECT salt FROM users where username = @username";
            string salt = "";
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection("Server=bootcamp-tht.sre.wize.mx;Database=bootcamp_tht;User Id=secret;Password=noPow3r"))
            {
                salt = await connection.QueryFirstOrDefaultAsync<string>(sqlSalt, new { username = username });
            }
            return salt;
        }
    }
}
