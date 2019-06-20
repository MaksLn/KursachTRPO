using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPI
{
    public class Auth
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "http://localhost:44389/"; 
        const string KEY = "mysupersecret_secretkey!123";  
        public const int LIFETIME = 20; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
