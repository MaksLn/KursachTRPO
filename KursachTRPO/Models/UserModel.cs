using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models
{
    public class UserModel
    {
        public string Name { set; get; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
