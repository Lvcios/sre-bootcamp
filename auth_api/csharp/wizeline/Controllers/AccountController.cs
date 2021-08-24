using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using wizeline.Common;
using wizeline.Model;
using wizeline.Service;

namespace wizeline.Controllers
{
    [Route("")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private AccountService _accountService;

        //Here we can inject the service, it is not a big effort
        public AccountController()
        {
            _accountService = new AccountService();
        }

        [HttpGet]
        [Route("/")]
        [Route("/healthz")]
        public async Task<IActionResult> Health()
        {
            await Task.Delay(1000);
            return Ok("Ok");
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                return BadRequest("Please send username and password");
            
            var salt = await _accountService.GetUserSalt(loginRequest.Username);

            if (string.IsNullOrEmpty(salt))
                return NotFound();

            var user = await _accountService.AuthenticateUser(loginRequest.Username, loginRequest.Password, salt);
            
            if(user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("/protected")]
        public async Task<IActionResult> ProtectedEndpoint()
        {
            await Task.Delay(1000);
            return Ok(new { data = "You are under protected data" });
        }


        //[NonAction]
        //public async Task<UserModel> GetUserFromDb(string userName, string password)
        //{
        //    string sqlSalt = "SELECT username, salt FROM users where username = @username";
        //    var user = new UserModel();
        //    user = null;
        //    using (var connection = new MySql.Data.MySqlClient.MySqlConnection("Server=bootcamp-tht.sre.wize.mx;Database=bootcamp_tht;User Id=secret;Password=noPow3r"))
        //    {
        //        var userSalt = await connection.QueryFirstOrDefaultAsync<UserModel>(sqlSalt, new { username = userName });
        //        if(userSalt != null)
        //        {
        //            string pass = EncryptionFunctions.GetSHA256(userSalt.Salt, password);
        //            string sqlUser = "SELECT username, role FROM users where username = @username and password = @password";
        //            user = await connection.QueryFirstOrDefaultAsync<UserModel>(sqlUser, new { username = userName, password = pass});
        //        }
        //    }

        //    return user;
        //}
    }
}
