using KursachTRPO.Models.bdModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KursachTRPO.Models
{
    public class DataBaseContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<HistorySkips> HistorySkips { get; set; }
        public DbSet<History> Histories { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Email = "admin@mail.ru", Login="admin", Password = "123456", RoleId = adminRole.Id };

            Student student1 = new Student {Id=1, Name = "Maks", GroupId = 1, Address = "420", MidleName = "Mazur", LastName = "Aleksandrovich", NumberOfBook = "43253456" };
            Student student2 = new Student {Id=2, Name = "Maks", GroupId = 1, Address = "323", MidleName = "Mazur", LastName = "Aleksandrovich", NumberOfBook = "43345456" };

            Group group1 = new Group { Id = 1, Name = "16-ИТ-2", Specialty = "IT", CreateYear = DateTime.Now };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            modelBuilder.Entity<Group>().HasData(new Group[] { group1 });
            modelBuilder.Entity<Student>().HasData(new Student[] { student1,student2 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
