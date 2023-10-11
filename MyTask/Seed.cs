using MyTask.DataModels;

namespace MyTask
{
    public class Seed
    {
        private readonly DatabaseContext _dbContext;
        
        public Seed(DatabaseContext dbContext) {  _dbContext = dbContext; }

        public void SeedDatabaseContext()
        {
            if (!_dbContext.Users.Any())
            {
                var users = new List<User>()
                {
                    new User()
                    {
                        UserName = "Ivan",
                        UserAge = 25,
                        UserEmail = "ivan@mail.ru"
                    },
                    new User()
                    {
                        UserName = "Misha",
                        UserAge = 23,
                        UserEmail = "misha@mail.ru"
                    },
                    new User()
                    {
                        UserName = "Maxim",
                        UserAge = 27,
                        UserEmail = "maxim@mail.ru"
                    }
                };
                var roles = new List<Role>()
                {
                    new Role()
                    {
                        RoleName="user"
                    },
                    new Role()
                    {
                        RoleName="admin"
                    },
                    new Role()
                    {
                        RoleName="support"
                    },
                    new Role()
                    {
                        RoleName="supperadmin"
                    }
                };
                var userRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = users[0].UserId,
                        RoleId = roles[0].RoleId,
                        User = users[0],
                        Role = roles[0]
                    },
                    new UserRole()
                    {
                        UserId = users[0].UserId,
                        RoleId = roles[1].RoleId,
                        User = users[0],
                        Role = roles[1]
                    },
                    new UserRole()
                    {
                        UserId = users[1].UserId,
                        RoleId = roles[2].RoleId,
                        User = users[1],
                        Role = roles[2]
                    },
                    new UserRole()
                    {
                        UserId = users[2].UserId,
                        RoleId = roles[3].RoleId,                       
                        User = users[2],
                        Role = roles[3]
                    }
                };
                _dbContext.Users.AddRange(users);
                _dbContext.Roles.AddRange(roles);
                _dbContext.UserRoles.AddRange(userRoles);
                _dbContext.SaveChanges();
            }
        }
    }
}
