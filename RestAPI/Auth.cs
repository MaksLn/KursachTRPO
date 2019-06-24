using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute() : base(typeof(ClaimRequirementFilter))
        {
        }
    }


    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Claims.ToList()[2].Value.ToString() != context.HttpContext.Connection.RemoteIpAddress.ToString())
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

    public class ChekIpUser : AuthorizeFilter
    {
        public ChekIpUser(AuthorizationPolicy policy) : base(policy)
        {
        }

        public override Task OnAuthorizationAsync(Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Claims.ToList()[2].Value == context.HttpContext.Connection.RemoteIpAddress.ToString())
            {
                return Task.FromResult(AuthorizationResult.Failed());
            }
            // If there is another authorize filter, do nothing
            return base.OnAuthorizationAsync(context);
        }

    }
}
