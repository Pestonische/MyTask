using MyTask.DataModels;
using MyTask.Interfaces;

namespace MyTask.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DatabaseContext _context;

        public UserRoleRepository(DatabaseContext context)
        {
            _context = context;
        }

        public bool AddNewUserRole(int? userId, int? roleId)
        {
            User user = _context.Users.AsQueryable().Where(a => a.UserId == userId).FirstOrDefault();
            Role role = _context.Roles.AsQueryable().Where(a => a.RoleId == roleId).FirstOrDefault();
            var UserRole = new UserRole()
            {
                UserId = userId,
                RoleId = roleId,
                User = user,
                Role = role
            };
            _context.Add(UserRole);
            return Save();
        }

        public ICollection<UserRole> GetByUserId(User user)
        {
            return _context.UserRoles.AsQueryable().Where(x => x.UserId == user.UserId).ToList();
        }

        public ICollection<UserRole>? GetUserRoles()
        {
            return _context.UserRoles.AsQueryable().OrderBy(p => p.UserRolesId).ToList();
        }
        
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUserRole(UserRole oldUserRole, int roleId)
        {
            oldUserRole.RoleId = roleId;
            oldUserRole.Role = _context.Roles.AsQueryable().Where(c => c.RoleId == roleId).FirstOrDefault();
            _context.Update(oldUserRole);
            return Save();
        }

        public bool DeleteUserRoles(List<UserRole> userRole)
        {
            _context.RemoveRange(userRole);
            return Save();
        }

       
    }
}
