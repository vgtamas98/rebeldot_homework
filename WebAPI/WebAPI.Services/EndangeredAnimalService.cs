
using WebAPI.Common.Dto;
using WebAPI.Interfaces;
using WebAPI.Common.Util;

namespace WebAPI.Services
{
    public class EndangeredAnimalService: IEndangeredAnimalService
    {
        private IEndangeredAnimalRepository _endangeredAnimalRepository;
        
        public EndangeredAnimalService(IEndangeredAnimalRepository endangeredAnimalRepository)
        {
            _endangeredAnimalRepository = endangeredAnimalRepository;
            
        }

        public EndangeredAnimalDto GetById(string id)
        {
            return _endangeredAnimalRepository.GetById(id).ToDto();
        }

        public EndangeredAnimalDto Create(EndangeredAnimalDto dto, string userId)
        {
            var endangeredAnimal = dto.ToEntity();
            endangeredAnimal.CreatedBy = userId;
            return _endangeredAnimalRepository.Create(endangeredAnimal).ToDto();
        }

        public EndangeredAnimalDto Update(EndangeredAnimalDto dto)
        {
            return _endangeredAnimalRepository.Update(dto.ToEntity()).ToDto();
        }
        public bool RemoveById(string id)
        {
            return _endangeredAnimalRepository.RemoveById(id);
        }
    }
}
