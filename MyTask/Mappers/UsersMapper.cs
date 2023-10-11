using System;
using System.Collections.Generic;
using MyTask.Models;
using MyTask.DataModels;
using System.Linq;
using System.Data;

namespace MyTask.Mappers
{
    public class UsersMapper 
    {
        public static User? Map(UsersModel usersModel)
        {
            if (usersModel == null) return null;
                   
            return new User
            {
                UserId = usersModel.UserId,
                UserName = usersModel.UserName,
                UserAge = usersModel.UserAge,
                UserEmail = usersModel.UserEmail
            };
        }

        public static UsersModel? Map(User users)
        {
            if (users == null) return null;

            return new UsersModel
            {
                UserId = users.UserId,
                UserName = users.UserName,
                UserAge = users.UserAge,
                UserEmail = users.UserEmail
            };

        }

        public static Role? Map(RolesModel rolesModel)
        {
            if (rolesModel == null) return null;

            return new Role
            {
                RoleId = rolesModel.RoleId,
                RoleName = rolesModel.RoleName
            };
        }
        public static RolesModel? Map(Role role)
        {
            if (role == null) return null;

            return new RolesModel
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }

        public static List<UserRole>? Map(UserRolesModel userRoleModel)
        {
            if (userRoleModel == null) return null;

            List<UserRole> result = new List<UserRole>();

            foreach (var item in userRoleModel.RolesModel)
            {
                result.Add(new UserRole()
                {
                    UserId = userRoleModel.User.UserId,
                    RoleId = item.RoleId
                });
            }

            return result;
        }
        public static UserRolesModel? Map(User user, List<Role> roles)
        {
            if (user == null || roles == null) return null;

            UserRolesModel userRolesModel = new UserRolesModel()
            {
                User = Map(user),
                RolesModel = new List<RolesModel>()
            };
            
            List<RolesModel> rolesModel = new List<RolesModel>();

            foreach (var role in roles)
            {
                userRolesModel.RolesModel.Add(Map(role));
            } 
            return userRolesModel;
        }

    }
}
