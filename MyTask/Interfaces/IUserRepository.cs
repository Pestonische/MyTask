using MyTask.DataModels;
using MyTask.Models;

namespace MyTask.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers(PageParams pageParams);
        User GetUserByUserId(int userId);
        User GetUserByEmail(string userEmail);
        bool CreateUser(List<int> rolesId, User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool UserExists(int userId);
        bool Save();
    }
}
