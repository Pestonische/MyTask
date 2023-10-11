using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTask.Models
{
    public class RolesModel
    {        
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
