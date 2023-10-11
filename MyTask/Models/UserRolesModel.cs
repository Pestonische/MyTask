using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTask.Models
{
    public class UserRolesModel
    {
        public UsersModel User { get; set; }
        public List<RolesModel> RolesModel { get; set;}
    }
}
