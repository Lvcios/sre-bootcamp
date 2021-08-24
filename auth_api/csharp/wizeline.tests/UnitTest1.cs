using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using wizeline.Common;
using wizeline.Controllers;
using wizeline.Model;
using wizeline.Service;

namespace wizeline.tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test_Health_Node_Ok()
        {
            var controller = new AccountController();
            var response = (Microsoft.AspNetCore.Mvc.OkObjectResult)await controller.Health();
            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        }


        [Test]
        [TestCase("F^S%QljSfV","secret", "857c8a8fd0531d50f88ec79a9270ff419cbe17a2d4e32711e27694596d21efd2")]
        [TestCase("KjvFUC#K*i","noPow3r", "ffb08b796ebe009ba724b5cbabd49076c276f06ba85c0020b0dde2d447ae2c5f")]
        [TestCase("F^S%QljSfV","thisIsNotAPasswordBob", "3e4b6019e5cbb8fa09adffbadc272e2b6a62660f792d622df41b39385088e469")]
        [TestCase("f1nd1ngn3m0","farm1990M0O", "07dbb6e6832da0841dd79701200e4b179f1a94a7b3dd26f612817f3c03117434")]
        public void Test_Get_Password_With_SHA2_Ok(string salt, string password, string expected)
        {
            var hashedPassword = EncryptionFunctions.GetSHA256(salt, password);
            Assert.AreEqual(expected, hashedPassword);
        }

        [Test]
        [TestCase("admin", "secret")]
        [TestCase("noadmin", "noPow3r")]
        [TestCase("bob", "thisIsNotAPasswordBob")]
        public async Task Test_Login_Returns_OK_With_Token(string username, string password)
        {
            var controller = new AccountController();
            var response = (Microsoft.AspNetCore.Mvc.OkObjectResult)await controller.Login(new LoginModel { Password = password, Username = username });
            
            var user = (UserModel)response.Value;

            Assert.AreEqual(typeof(UserModel), response.Value.GetType());
            Assert.NotNull(response.Value);
        }


        [Test]
        public async Task Test_Protected_With_Login()
        {
            var controller = new AccountController();
            var response = (Microsoft.AspNetCore.Mvc.OkObjectResult)await controller.ProtectedEndpoint();

            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(new { data = "You are under protected data" }.ToString(), response.Value.ToString());
        }


        [Test]
        [TestCase("admin1", "secret1")]
        [TestCase("noadmin1", "noPow3r1")]
        [TestCase("bob1", "thisIsNotAPasswordBob1")]
        public async Task Test_Login_BadUser_And_BadPass_Returns_NotFound(string username, string password)
        {
            var controller = new AccountController();
            var response = (Microsoft.AspNetCore.Mvc.NotFoundResult)await controller.Login(new LoginModel { Username = "admin", Password = "F^S%QljSfV" });
            Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        [TestCase("admin", "secret1")]
        [TestCase("noadmin", "noPow3r1")]
        [TestCase("bob", "thisIsNotAPasswordBob1")]
        public async Task Test_Login_RightUser_BadPass_Returns_NotFound(string username, string password)
        {
            var controller = new AccountController();
            var response = (Microsoft.AspNetCore.Mvc.NotFoundResult)await controller.Login(new LoginModel { Username = "admin", Password = "F^S%QljSfVss" });
            Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
        }


    }
}