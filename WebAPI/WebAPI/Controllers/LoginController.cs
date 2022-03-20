
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Text;
using WebAPI.API.Middleware.Auth;
using WebAPI.API.Model;
using WebAPI.API.Models;
using WebAPI.API.Util;
using WebAPI.Common.Dto;
using WebAPI.Interfaces;

namespace WebAPI.API.Controllers
{
    /// <summary>
    /// Authenticate users
    /// </summary>
    [Route("api/v2/login")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        public IActionResult LoginEmail([FromBody] AuthenticateRequest login)
        {
            IActionResult response = Unauthorized();
            var token = _userService.Authenticate(login.Email, login.Password);

            if (token != null)
            {
                response = Ok(token);
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                if (claimValue != null)
                {
                    var user = _userService.GetByEmail(claimValue);
                    HttpContext.Items.Add("User", user);
                }
            }
                

            return response;
        }

        [HttpGet]
        [Authorize(Policy = Policies.All)]
        public IActionResult RenewToken()
        {
            var userDto = (UserDto)HttpContext.Items["userSession"];
            var refreshedToken = _userService.RefreshToken(userDto);
            return Ok(refreshedToken);
        }
    }
}
