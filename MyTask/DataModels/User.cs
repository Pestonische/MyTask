using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTask.DataModels
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UserAge { get; set; }
        public string UserEmail { get; set; }
    }
}
