using Microsoft.AspNetCore.Mvc;
using WebAPI.API.Model;
using WebAPI.Interfaces;
using WebAPI.API.Util;
using Microsoft.AspNetCore.Authorization;
using WebAPI.API.Middleware.Auth;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Common.Dto;

namespace WebAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndangeredAnimalController : ControllerBase
    {
        private IEndangeredAnimalService _endangeredAnimalService;
        private IUserService _userService;

        public EndangeredAnimalController(IEndangeredAnimalService endangeredAnimalService, IUserService userService)
        {
            _endangeredAnimalService = endangeredAnimalService;
            _userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = Policies.Admin)]

        public IActionResult Get([FromRoute] string id)
        {
            if (id == "")
            {
                return BadRequest("Missing id");
            }
            var result = _endangeredAnimalService.GetById(id);
            return result == null ? NotFound() : Ok(result.ToModel());
        }

        /// <summary>
        /// Creating new EndangeredAnimal
        /// </summary>
        /// <param name="EndangeredAnimal"> From the body comes the new EndangeredAnimal's data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.Admin)]

        public IActionResult Post([FromBody] EndangeredAnimal endangeredAnimal)
        {
            

            var userDto = (UserDto)HttpContext.Items["userSession"];
            var dto = endangeredAnimal.ToDto();
            var result = _endangeredAnimalService.Create(dto, userDto.Id);
            return Ok(result.ToModel());

        }

      /// <summary>
      /// Modifying EndangeredAnimal's data
      /// </summary>
      /// <param name="EndangeredAnimal"> From the body comes the new data</param>
      /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult Update( [FromBody] EndangeredAnimal endangeredAnimal) 
        {
            var result = _endangeredAnimalService.Update(endangeredAnimal.ToDto());
            return Ok(result.ToModel());
        }


        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult Delete([FromRoute] string id)
        {
            if (id == "")
            {
                return BadRequest("Missing id");
            }
            var result = _endangeredAnimalService.RemoveById(id);
            if (result)
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        private User GetUserFromToken(string accesToken)
        {
            var token = accesToken.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            if (claimValue != null)
            {
                var user = _userService.GetByEmail(claimValue);
                return user.ToModel();
            }
            return null;
        }
    }
}
