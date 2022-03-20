using WebAPI.Common.Dto;
using WebAPI.Common.Entities;

namespace WebAPI.Common.Util
{
    public static class MapperUtil
    {
        public static EndangeredAnimal ToEntity(this EndangeredAnimalDto dto)
        {
            return new EndangeredAnimal
            {
                Id = dto.Id,
                Name = dto.Name,
                }; 
        }

        public static EndangeredAnimalDto ToDto(this EndangeredAnimal EndangeredAnimal)
        {
            return new EndangeredAnimalDto
            {
                Id = EndangeredAnimal.Id,
                Name = EndangeredAnimal.Name,
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

        public static User ToEntity(this UserDto dto)
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


    }
}
