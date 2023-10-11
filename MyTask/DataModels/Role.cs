using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTask.DataModels
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
