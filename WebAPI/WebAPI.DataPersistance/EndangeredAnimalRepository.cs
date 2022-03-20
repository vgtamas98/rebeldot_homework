using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Common.Entities;
using WebAPI.Interfaces;

namespace WebAPI.DataPersistance
{
    public class EndangeredAnimalRepository : IEndangeredAnimalRepository
    {
        private IList<EndangeredAnimal> _endangeredAnimals;

        public EndangeredAnimalRepository()
        {
            _endangeredAnimals = new List<EndangeredAnimal>();
        }

        public EndangeredAnimal GetById(string id)
        {
            return _endangeredAnimals.Where(x => x.Id == id).First();
        }

        
        public EndangeredAnimal Create(EndangeredAnimal endangeredAnimal)
        {
            endangeredAnimal.Id = Guid.NewGuid().ToString();
            _endangeredAnimals.Add(endangeredAnimal);
            return endangeredAnimal;
        }

        public EndangeredAnimal Update(EndangeredAnimal endangeredAnimal)
        {
            var existingEntity = GetById(endangeredAnimal.Id);
            if (existingEntity != null)
            {
                _endangeredAnimals.Remove(existingEntity);
                _endangeredAnimals.Add(endangeredAnimal);
                return endangeredAnimal;
            }
            return Create(endangeredAnimal);
        }

        public bool RemoveById(string id)
        {
            var entity = _endangeredAnimals.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
            {
                return false;
            }
            return _endangeredAnimals.Remove(entity);
        }
    }
}
