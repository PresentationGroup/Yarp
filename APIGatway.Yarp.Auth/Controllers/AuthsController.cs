using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIGatway.Yarp.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(string name, string myClaimValue, string returnUrl)
        {
            // Create a new identity with 2 claims based on the fields in the form
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim("myCustomClaim", myClaimValue)
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            return SignIn(principal, new AuthenticationProperties()
            {
                RedirectUri = returnUrl
                // SignIn is the only one that requires a scheme: https://github.com/dotnet/aspnetcore/issues/23325
            }, CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
