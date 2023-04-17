using Microsoft.EntityFrameworkCore;
using MyEmp.Models;

namespace MyEmp.MasterdataMigration
{
    public static class masterMigration
    {
        public static void StateMigration(this ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<CoreState>().HasData
                (
                    new CoreState { Id=1,StateName="West Bengal"},
                    new CoreState { Id=2, StateName= "Odisha" },
                    new CoreState { Id =3, StateName = "Karnataka" },
                    new CoreState { Id= 4, StateName = "Tamil Nadu" },
                    new CoreState { Id= 5,StateName = "Delhi" }
                );
        
        }

        public static void TechNamesMigration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoreTechNames>().HasData
                (
                new CoreTechNames { Id = 1, Tech_Name = "React" },
                new CoreTechNames { Id = 2, Tech_Name ="NodeJs" },
                new CoreTechNames { Id= 3 ,Tech_Name=".Net"},
                new CoreTechNames { Id=4, Tech_Name ="Angular"}
                 );
        }
    }
}
