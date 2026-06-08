using Microsoft.EntityFrameworkCore;
using EmployeeApi.Models;

namespace Employees.DbContxt

{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("tblEmployee");
                entity.HasKey(p => p.EmpId);
                entity.Property(p => p.EmpId).ValueGeneratedOnAdd();
                entity.Property(e => e.EmpName).HasMaxLength(20);
                entity.Property(e => e.DeptId).HasMaxLength(20);
                entity.Property(e => e.LocId).HasMaxLength(20);
                // Foreign key relationships
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employee)
                      .HasForeignKey(e => e.DeptId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Location)
                      .WithMany(l => l.Employee)
                      .HasForeignKey(e => e.LocId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

         

            base.OnModelCreating(modelBuilder);
        }

    }

    
}
