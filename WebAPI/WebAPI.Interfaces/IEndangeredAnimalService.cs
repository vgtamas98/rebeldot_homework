using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Dto;

namespace WebAPI.Interfaces
{
    public interface IEndangeredAnimalService
    {
        public EndangeredAnimalDto GetById(string id);
        public EndangeredAnimalDto Create(EndangeredAnimalDto dto, string userId);
        public EndangeredAnimalDto Update(EndangeredAnimalDto dto);
        public bool RemoveById(string id);
    }
}
