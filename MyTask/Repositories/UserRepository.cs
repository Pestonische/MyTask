using MyTask.DataModels;
using MyTask.Interfaces;
using MyTask.Models;

namespace MyTask.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public bool CreateUser(List<int> roleId, User user)
        {
            _context.Add(user);
            var userRoles = new List<UserRole>();
            foreach (var id in roleId) 
            {
                userRoles.Add(new UserRole() {
                    UserId = user.UserId,
                    RoleId = id, 
                    User = user,
                    Role = _context.Roles.AsQueryable().Where(a => a.RoleId == id).FirstOrDefault()
                });               
            }
            _context.UserRoles.AddRange(userRoles);
            return Save();
        }

        public User GetUserByUserId(int userId)
        {
            return _context.Users.AsQueryable().Where(p => p.UserId == userId).FirstOrDefault();
        }

        public ICollection<User> GetUsers(PageParams pageParams)
        {
            return PageMetadata<User>.ToPagedSortList(_context.Users.AsQueryable().OrderBy(p => p.UserId), pageParams.OrderBy, pageParams.OrderAsc);
        }

        public bool UserExists(int userId)
        {
            return _context.Users.AsQueryable().Any(p => p.UserId == userId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public User GetUserByEmail(string userEmail)
        {
            return _context.Users.AsQueryable().Where(p => p.UserEmail == userEmail).FirstOrDefault();
        }

        public bool UpdateUser(User user)
        {
           _context.Update(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {            
            _context.Remove(user);
            return Save();
        }
    }
}
