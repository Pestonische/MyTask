using MyTask.DataModels;
using MyTask.Interfaces;

namespace MyTask.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DatabaseContext _context;

        public RoleRepository(DatabaseContext context) {
           _context = context;
        }

        public Role GetRoleByRoleId(int? roleId)
        {
            return _context.Roles.AsQueryable().Where(p => p.RoleId == roleId).FirstOrDefault();
        }

        public ICollection<Role> GetRoles()
        {
            return _context.Roles.AsQueryable().OrderBy(p => p.RoleId).ToList();
        }

        public bool RoleExists(int roleId)
        {
            return _context.Roles.AsQueryable().Any(p => p.RoleId == roleId);
        }
    }
}
