using WebAPI.Common.Entities;


namespace WebAPI.Interfaces
{
    public interface IUserRepository
    {   
        public List<User> Users();
        public User GetByUserName(string UserName);
        public User GetByEmail(string email);

    }
}
