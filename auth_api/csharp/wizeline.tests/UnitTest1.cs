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
        [TestCase("F^S%QljSfV","secret", "15e24a16abfc4eef5faeb806e903f78b188c30e4984a03be4c243312f198d1229ae8759e98993464cf713e3683e891fb3f04fbda9cc40f20a07a58ff4bb00788")]
        [TestCase("KjvFUC#K*i","noPow3r", "89155af89e8a34dcbde088c72c3f001ac53486fcdb3946b1ed3fde8744ac397d99bf6f44e005af6f6944a1f7ed6bd0e2dd09b8ea3bcfd3e8862878d1709712e5")]
        [TestCase("F^S%QljSfV","thisIsNotAPasswordBob", "2c9dab627bd73b6c4be5612ff77f18fa69fa7c2a71ecedb45dcec45311bea736e320462c6e8bfb2421ed112cfe54fac3eb9ff464f3904fe7cc915396b3df36f0")]
        public void Test_Get_Password_With_SHA2_Ok(string salt, string password, string expected)
        {
            var hashedPassword = EncryptionFunctions.GetSHA512(salt, password);
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