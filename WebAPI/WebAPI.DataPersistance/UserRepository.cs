
using WebAPI.Common.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private List<User> users = new List<User>{
            new User() {Id = "1", Name = "Varga Tamas", UserName = "tamasvarga", Password = "password", Email = "tamas.varga@rebeldot.com", Age = 23, UserRole = "Admin" },
            new User() {Id = "2", Name = "First Last", UserName = "firstlast", Password = "password", Email = "example@domain.com", Age = 23, UserRole = "User" }
        };

        public List<User> Users()
        {
            return users;
        }
        public User GetById(string id)
        {
            return users.Where(x => x.Id == id).First();
        }

        public User GetByUserName(string username)
        {
            return users.Where(x => x.UserName == username).First();
        }

        public User GetByEmail(string email)
        {
            return users.Where(x => x.Email == email).First();
        }
        public User Create(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            users.Add(user);
            return user;
        }

        public User Update(User user)
        {
            var existingEntity = GetById(user.Id);
            if (existingEntity != null)
            {
                users.Remove(existingEntity);
                users.Add(user);
                return user;
            }
            return Create(user);
        }

        public bool RemoveById(string id)
        {
            var entity = users.Where(x => x.Id == id).FirstOrDefault();
            if (entity == null)
            {
                return false;
            }
            return users.Remove(entity);
        }
    }
}
