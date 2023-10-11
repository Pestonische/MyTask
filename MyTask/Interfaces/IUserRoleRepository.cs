using MyTask.DataModels;

namespace MyTask.Interfaces
{
    public interface IUserRoleRepository
    {
        ICollection<UserRole> GetByUserId(User user);
        bool AddNewUserRole (int? userId, int? roleId);
        bool UpdateUserRole(UserRole oldUserRole, int roleId);
        bool DeleteUserRoles(List<UserRole> userRole);
        ICollection<UserRole>? GetUserRoles();
        bool Save();
    }
}
