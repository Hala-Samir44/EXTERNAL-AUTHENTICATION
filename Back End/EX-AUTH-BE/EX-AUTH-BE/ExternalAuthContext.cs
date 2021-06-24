using EX_AUTH_BE.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE
{
    public class ExternalAuthContext : DbContext
    {

        public ExternalAuthContext(DbContextOptions<ExternalAuthContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasIndex(b => b.Title)
                .IsUnique();
            modelBuilder.Entity<Permission>()
                .HasIndex(b => b.Title)
                .IsUnique();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<ExternalLogins> ExternalLogins { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }


    }
}
