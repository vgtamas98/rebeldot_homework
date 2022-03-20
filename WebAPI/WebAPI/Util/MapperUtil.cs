
using WebAPI.API.Model;
using WebAPI.Common.Dto;

namespace WebAPI.API.Util
{
    public static class MapperUtil
    {
        public static EndangeredAnimal ToModel(this EndangeredAnimalDto dto)
        {
            return new EndangeredAnimal
            {
                Id = dto.Id,
                Name = dto.Name
            }; 
        }

        public static EndangeredAnimalDto ToDto(this EndangeredAnimal endangeredAnimal)
        {
            return new EndangeredAnimalDto
            {
                Id = endangeredAnimal.Id,
                Name = endangeredAnimal.Name
            };
        }

        public static User ToModel(this UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                UserName = dto.UserName,
                Age = dto.Age,
                UserRole = dto.UserRole,
            };
        }

        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Age = user.Age,
                UserRole = user.UserRole,
            };
        }
    }
}
