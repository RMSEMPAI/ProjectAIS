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

            //modelBuilder.Entity<ITEmployee>()
            //    .HasOne(e => e.Language)           // У сотрудника один язык
            //    .WithMany(l => l.Employees)        // У языка много сотрудников
            //    .HasForeignKey(e => e.LanguageId)  // Внешний ключ
            //    .OnDelete(DeleteBehavior.Restrict); // Запрещаем удаление языка если есть сотрудники

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