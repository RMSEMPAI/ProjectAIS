using LogicLib;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ITEmployeeContext : DbContext
    {
        public ITEmployeeContext(DbContextOptions<ITEmployeeContext> options) : base(options) { }

        public DbSet<ITEmployee> ITEmployees { get; set; }
        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ITEmployee>()
                .ToTable("ITEmployee") 
                .HasKey(e => e.Id);

            modelBuilder.Entity<ITEmployee>()
                .Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<ITEmployee>()
                .Property(e => e.Position)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<ITEmployee>()
                .Property(e => e.Department)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<ITEmployee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<ITEmployee>()
                .Property(e => e.ExperienceYears)
                .IsRequired();

            // Конфигурация для Language
            modelBuilder.Entity<Language>()
                .ToTable("Languages")
                .HasKey(l => l.Id);

            modelBuilder.Entity<Language>()
                .Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}