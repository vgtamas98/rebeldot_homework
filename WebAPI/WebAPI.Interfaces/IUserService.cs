
using WebAPI.Common.Dto;
using WebAPI.Common.Entities;

namespace WebAPI.Interfaces
{
    public interface IUserService
    {
        public string Authenticate(string email, string password);

        public UserDto GetByEmail(string email);
        public string RefreshToken(UserDto dto);
    }
}
