using MyTask.DataModels;

namespace MyTask.Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetRoles();
        Role GetRoleByRoleId(int? roleId);
        bool RoleExists(int roleId);
    }
}
