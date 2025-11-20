using Dapper;
using DataAccessLayer;
using LogicLib;
using LogicLibrary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DBCreate
{
    // Временный класс для десериализации JSON
    public class TempEmployee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Position Position { get; set; }
        public Department Department { get; set; }
        public decimal Salary { get; set; }
        public int ExperienceYears { get; set; }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var jsonFilePath = "C:\\Users\\stepa\\AppData\\Roaming\\data.json";

                if (!File.Exists(jsonFilePath))
                {
                    Console.WriteLine($"Файл {jsonFilePath} не найден!");
                    return;
                }

                var connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stepa\\source\\repos\\Khomkolova\\ProjectAIS\\DataAccessLayer\\Database1.mdf;Integrated Security=True;";

                CreateTablesIfNotExists(connectionString);

                Console.WriteLine("Чтение JSON файла...");
                var jsonData = File.ReadAllText(jsonFilePath);

                // ДЕСЕРИАЛИЗАЦИЯ через временный класс
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                var tempData = JsonSerializer.Deserialize<List<TempEmployee>>(jsonData, options);

                if (tempData == null)
                {
                    Console.WriteLine("ОШИБКА: Не удалось десериализовать JSON");
                    return;
                }

                if (!tempData.Any())
                {
                    Console.WriteLine("Нет данных для импорта");
                    return;
                }

                Console.WriteLine($"Найдено {tempData.Count} записей в JSON файле");

                // Проверяем первый сотрудник после преобразования
                var firstEmployee = tempData.First();
                Console.WriteLine($"Первый сотрудник: ID={firstEmployee.Id}, Name='{firstEmployee.FullName}'");

                // Получаем доступные языки
                var availableLanguages = GetLanguages(connectionString);

                if (!availableLanguages.Any())
                {
                    Console.WriteLine("ОШИБКА: В таблице Languages нет данных!");
                    return;
                }

                Console.WriteLine($"Доступно языков в базе: {availableLanguages.Count}");

                int successCount = 0;
                int skippedCount = 0;
                var random = new Random();

                // Используем Dapper для вставки данных
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                foreach (var employee in tempData)
                {
                    try
                    {
                        // Проверяем, что данные корректны
                        if (employee.Id == 0 || string.IsNullOrWhiteSpace(employee.FullName))
                        {
                            Console.WriteLine($"❌ Пропуск сотрудника - некорректные данные: ID={employee.Id}, Name='{employee.FullName}'");
                            skippedCount++;
                            continue;
                        }

                        // Проверяем, существует ли уже сотрудник с таким ID
                        var existing = connection.ExecuteScalar<int?>(
                            "SELECT COUNT(*) FROM ITEmployee WHERE Id = @Id",
                            new { employee.Id });

                        if (existing > 0)
                        {
                            Console.WriteLine($"⚠️ Сотрудник с ID {employee.Id} уже существует - пропуск");
                            skippedCount++;
                            continue;
                        }

                        // Назначаем случайный LanguageId
                        var languageId = availableLanguages[random.Next(availableLanguages.Count)].Id;

                        // Вставляем сотрудника через Dapper
                        connection.Execute(@"
                            INSERT INTO ITEmployee (Id, FullName, Position, Department, Salary, ExperienceYears, LanguageId)
                            VALUES (@Id, @FullName, @Position, @Department, @Salary, @ExperienceYears, @LanguageId)",
                            new
                            {
                                employee.Id,
                                employee.FullName,
                                Position = employee.Position.ToString(),
                                Department = employee.Department.ToString(),
                                employee.Salary,
                                employee.ExperienceYears,
                                LanguageId = languageId
                            });

                        var language = availableLanguages.First(l => l.Id == languageId);
                        Console.WriteLine($"✅ Добавлен: ID={employee.Id}, Name='{employee.FullName}', Language={language.Name}");
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Ошибка при добавлении ID {employee.Id}: {ex.Message}");
                        if (ex.InnerException != null)
                            Console.WriteLine($"   Внутренняя ошибка: {ex.InnerException.Message}");
                        skippedCount++;
                    }
                }

                // Получаем общее количество сотрудников
                var totalEmployees = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM ITEmployee");

                Console.WriteLine($"\n=== РЕЗУЛЬТАТ ИМПОРТА ===");
                Console.WriteLine($"✅ Успешно добавлено: {successCount} сотрудников");
                Console.WriteLine($"⚠️ Пропущено: {skippedCount} записей");
                Console.WriteLine($"📊 Всего в базе: {totalEmployees} сотрудников");

                if (successCount > 0)
                {
                    ShowLanguageStatistics(connection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Критическая ошибка: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"   Внутренняя ошибка: {ex.InnerException.Message}");
                Console.ReadKey();
            }
        }

        private static void CreateTablesIfNotExists(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                // Удаляем старые таблицы если существуют
                connection.Execute(@"
                    IF EXISTS (SELECT * FROM sysobjects WHERE name='ITEmployee' AND xtype='U')
                        DROP TABLE ITEmployee;
                    
                    IF EXISTS (SELECT * FROM sysobjects WHERE name='Languages' AND xtype='U')
                        DROP TABLE Languages;");

                // Таблица Languages
                var createLanguagesTableSql = @"
                    CREATE TABLE Languages (
                        Id INT PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL
                    )";

                connection.Execute(createLanguagesTableSql);
                Console.WriteLine("✅ Таблица Languages создана");

                // Таблица ITEmployee
                var createEmployeeTableSql = @"
                    CREATE TABLE ITEmployee (
                        Id INT PRIMARY KEY,
                        FullName NVARCHAR(200) NOT NULL,
                        Position NVARCHAR(50) NOT NULL,
                        Department NVARCHAR(50) NOT NULL,
                        Salary DECIMAL(18,2) NOT NULL,
                        ExperienceYears INT NOT NULL,
                        LanguageId INT NOT NULL,
                        CONSTRAINT FK_ITEmployee_Language FOREIGN KEY (LanguageId) REFERENCES Languages(Id)
                    )";

                connection.Execute(createEmployeeTableSql);
                Console.WriteLine("✅ Таблица ITEmployee создана");

                SeedLanguages(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при создании таблиц: {ex.Message}");
                throw;
            }
        }

        private static void SeedLanguages(SqlConnection connection)
        {
            var languages = new[]
            {
                new { Id = 0, Name = "Unknow"},
                new { Id = 1, Name = "C#" },
                new { Id = 2, Name = "Java" },
                new { Id = 3, Name = "Python" },
                new { Id = 4, Name = "JavaScript" },
                new { Id = 5, Name = "TypeScript" },
                new { Id = 6, Name = "Go" },
                new { Id = 7, Name = "Rust" },
                new { Id = 8, Name = "SQL" }
            };

            foreach (var lang in languages)
            {
                connection.Execute("INSERT INTO Languages (Id, Name) VALUES (@Id, @Name)", lang);
            }

            Console.WriteLine($"✅ Добавлено {languages.Length} языков программирования");

            Console.WriteLine("\n🗣️ Доступные языки:");
            var existingLanguages = connection.Query<Language>("SELECT * FROM Languages ORDER BY Id");
            foreach (var lang in existingLanguages)
            {
                Console.WriteLine($"  ID: {lang.Id} | {lang.Name}");
            }
        }

        private static List<Language> GetLanguages(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection.Query<Language>("SELECT * FROM Languages ORDER BY Id").ToList();
        }

        private static void ShowLanguageStatistics(SqlConnection connection)
        {
            var statistics = connection.Query(@"
                SELECT l.Name as Language, COUNT(e.Id) as EmployeeCount
                FROM Languages l
                LEFT JOIN ITEmployee e ON l.Id = e.LanguageId
                GROUP BY l.Id, l.Name
                ORDER BY EmployeeCount DESC, l.Name")
                .ToList();

            Console.WriteLine($"\n=== 📊 СТАТИСТИКА ПО ЯЗЫКАМ ===");

            foreach (var stat in statistics)
            {
                Console.WriteLine($"  {stat.Language}: {stat.EmployeeCount} сотрудников");
            }

            var mostPopular = statistics.OrderByDescending(s => s.EmployeeCount).First();
            if (mostPopular.EmployeeCount > 0)
            {
                Console.WriteLine($"\n🏆 Самый популярный язык: {mostPopular.Language} ({mostPopular.EmployeeCount} сотрудников)");
            }
        }
    }
}