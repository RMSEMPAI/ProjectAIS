using LogicLib;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ITEmployeeContext : DbContext
    {
        public ITEmployeeContext(DbContextOptions<ITEmployeeContext> options) : base(options) { }

        // ИСПРАВЛЕНО: Указываем точное имя таблицы
        public DbSet<ITEmployee> ITEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Явно указываем имя таблицы
            modelBuilder.Entity<ITEmployee>()
                .ToTable("ITEmployee") // ← ВАЖНО: указываем точное имя таблицы в БД
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
        }
    }
}