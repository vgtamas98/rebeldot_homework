using WebAPI.Common.Entities;

namespace WebAPI.Interfaces
{
    public interface IEndangeredAnimalRepository
    {
        public EndangeredAnimal GetById(string Id);

        public EndangeredAnimal Create(EndangeredAnimal endangeredAnimal);
        public EndangeredAnimal Update(EndangeredAnimal endangeredAnimal);
        public bool RemoveById(string Id);
    }
}
