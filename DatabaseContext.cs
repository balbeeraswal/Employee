using Microsoft.EntityFrameworkCore;
using Employees.Models;

namespace Employees.DbContxt

{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Employees.Models.Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees.Models.Employee>(entity =>
            {
                entity.ToTable("tblEmployee");
                entity.HasKey(p => p.EmpId);
                entity.Property(p => p.EmpId).ValueGeneratedOnAdd();
                entity.Property(e => e.EmpName).HasMaxLength(20);
                entity.Property(e => e.DeptId).HasMaxLength(20);
                entity.Property(e => e.LocId).HasMaxLength(20);
            });

         

            base.OnModelCreating(modelBuilder);
        }

    }

    
}
