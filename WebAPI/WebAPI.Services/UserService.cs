using System.Text;
using WebAPI.Common.Dto;
using WebAPI.Common.Entities;
using WebAPI.Common.Util;
using WebAPI.Interfaces;
using WebAPI.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Services
{
    

    public class UserService : IUserService
    {
        private readonly AuthorizationSettings _authorizationSettings;
        private IUserRepository _userRepository;
        public UserService( IUserRepository userRepository, IOptions<AuthorizationSettings> appSettings)
        {
            _userRepository = userRepository;
            _authorizationSettings = appSettings.Value;
        }

        public string? Authenticate(string email, string password)
        {
            var user = _userRepository.Users().FirstOrDefault(x => x.Email == email && x.Password == password);
            return user == null ? null : GenerateJwtToken(user);

        }

        public UserDto GetByEmail(string email)
        {
            return _userRepository.Users().FirstOrDefault(x => x.Email == email).ToDto();
        }

        public string RefreshToken(UserDto dto)
        {
            return GenerateJwtToken(dto.ToEntity());
        }
        private string GenerateJwtToken(User user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authorizationSettings.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email.ToString()),
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("role",user.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                 issuer: _authorizationSettings.Issuer,
                 audience: _authorizationSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_authorizationSettings.TokenLifeTimeInMinutes),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       

    }
}
