using DATN_Back_end.Extensions;
using DATN_Back_end.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Report>()
                .HasMany<Comment>(x => x.Comments);

            modelBuilder.Entity<User>()
                .HasData(new User()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Username = "admin",
                    Email = "lethiluuhieu@gmail.com",
                    Password = "admin123456".Encrypt(),
                    FirstName = "Admin",
                    LastName = "01",
                    Role = Role.Admin
                });

            modelBuilder.Entity<User>()
                .HasData(new User()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Username = "manager",
                    Email = "luuhieu8720@gmail.com",
                    Password = "manager123456".Encrypt(),
                    FirstName = "Manager",
                    LastName = "01",
                    Role = Role.Manager
                });

            modelBuilder.Entity<User>()
                .HasData(new User()
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Username = "employee",
                    Email = "capstone.project.ltlh@gmail.com",
                    Password = "employee123456".Encrypt(),
                    FirstName = "Employee",
                    LastName = "01",
                    Role = Role.Employee
                });

            modelBuilder.Entity<FormStatus>()
                .HasData(new FormStatus()
                {
                    Id = 1,
                    Status = "Pending"
                });

            modelBuilder.Entity<FormStatus>()
                .HasData(new FormStatus()
                {
                    Id = 2,
                    Status = "Approved"
                });

            modelBuilder.Entity<FormStatus>()
                .HasData(new FormStatus()
                {
                    Id = 3,
                    Status = "Rejected"
                });

            modelBuilder.Entity<RequestType>()
                .HasData(new RequestType() 
                { 
                    Id = 1,
                    TypeName = "Off full day"
                });

            modelBuilder.Entity<RequestType>()
                .HasData(new RequestType()
                {
                    Id = 2,
                    TypeName = "Off morning"
                });

            modelBuilder.Entity<RequestType>()
                .HasData(new RequestType()
                {
                    Id = 3,
                    TypeName = "Off afternoon"
                });

            modelBuilder.Entity<RequestType>()
                .HasData(new RequestType()
                {
                    Id = 4,
                    TypeName = "Off by hour"
                });
            modelBuilder.Entity<RequestType>()
                .HasData(new RequestType()
                {
                    Id = 5,
                    TypeName = "Lately checkin"
                });
            modelBuilder.Entity<RequestType>()
                .HasData(new RequestType()
                {
                    Id = 6,
                    TypeName = "Early checkout"
                });
        }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<FormRequest> FormRequests { get; set; }

        public virtual DbSet<Report> Reports { get; set; }

        public virtual DbSet<RequestType> RequestTypes { get; set; }

        public virtual DbSet<FormStatus> FormStatuses { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<Timekeeping> Timekeepings { get; set; }

        public virtual DbSet<ForgetPassword> ForgetPasswords { get; set; }
    }
}
