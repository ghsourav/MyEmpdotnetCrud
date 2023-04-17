using Microsoft.EntityFrameworkCore;
using MyEmp.MasterdataMigration;
using MyEmp.Models;
using System;


namespace MyEmp.Context
{
    public class MyCoreEmpContext:DbContext 
    {
        public MyCoreEmpContext(DbContextOptions<MyCoreEmpContext> options) : base(options)
        {

        }
        public DbSet<CoreState> CoreStates { get; set; }
        public DbSet<CoreEmployee> CoreEmployees { get; set; }

        public DbSet<CoreEmpTech> CoreEmpTeches { get; set;}

        public DbSet<CoreTechNames> CoreTechNames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoreEmployee>().ToTable("Employee");
            modelBuilder.Entity<CoreEmpTech>().ToTable("EmpTech");
            modelBuilder.Entity<CoreTechNames>().ToTable("Technames");
            modelBuilder.Entity<CoreState>().ToTable("States");
            modelBuilder.TechNamesMigration();
            modelBuilder.StateMigration();

           
        }
    }
}
