using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MyTask.DataModels
{
    public class DatabaseContext : DbContext
    {
        
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(User).Assembly);
        }
    }
}
