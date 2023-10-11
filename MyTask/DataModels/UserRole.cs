using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyTask.DataModels
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        public int UserRolesId { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

    }
}
