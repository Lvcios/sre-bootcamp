using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wizeline.Model
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
