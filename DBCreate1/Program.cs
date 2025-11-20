using Dapper;
using DataAccessLayer;
using LogicLib;
using LogicLibrary;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.Json;

namespace DBCreate1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var _mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lab1Data");

                // Создаем папку если не существует
                if (!Directory.Exists(_mainPath))
                {
                    Directory.CreateDirectory(_mainPath);
                    Console.WriteLine($"Создана папка: {_mainPath}");
                }

                var jsonFilePath = "C:\\Users\\stepa\\AppData\\Roaming\\data.json";

                // Проверяем существование JSON файла
                if (!File.Exists(jsonFilePath))
                {
                    Console.WriteLine($"Файл {jsonFilePath} не найден!");
                    Console.WriteLine("Создаем тестовые данные...");
                    CreateTestJsonFile(jsonFilePath);
                }

                // Добавляем обработку enum при десериализации
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new JsonStringEnumConverter() }
                };

                var jsonData = File.ReadAllText(jsonFilePath);
                var data = JsonSerializer.Deserialize<List<ITEmployee>>(jsonData, options);

                if (data == null || !data.Any())
                {
                    Console.WriteLine("Нет данных для импорта");
                    return;
                }

                var connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stepa\\source\\repos\\Khomkolova\\ProjectAIS\\DataAccessLayer\\Database1.mdf;Integrated Security=True;";

                // Создаем таблицы если не существуют
                CreateTablesIfNotExists(connectionString);

                var optionsBuilder = new DbContextOptionsBuilder<ITEmployeeContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using var context = new ITEmployeeContext(optionsBuilder.Options);

                // Создаем БД если не существует
                context.Database.EnsureCreated();

                var _iTEmployeeRepository = new EntityRepository<ITEmployee>(context);
                var _languageRepository = new EntityRepository<Language>(context);

                Console.WriteLine($"Найдено {data.Count} записей в JSON файле");

                // Получаем следующий доступный ID для сотрудников
                int nextEmployeeId = GetNextAvailableId(connectionString, "ITEmployee");
                int successCount = 0;
                int skippedCount = 0;

                foreach (var employee in data)
                {
                    try
                    {
                        // Проверяем валидность данных
                        if (string.IsNullOrWhiteSpace(employee.FullName))
                        {
                            Console.WriteLine($"Пропуск сотрудника - пустое имя");
                            skippedCount++;
                            continue;
                        }

                        // Генерируем новый ID если текущий = 0
                        if (employee.Id == 0)
                        {
                            employee.Id = nextEmployeeId++;
                        }

                        // Устанавливаем LanguageId по умолчанию если не установлен
                        if (employee.LanguageId == 0)
                        {
                            // Назначаем случайный язык из существующих
                            var languages = _languageRepository.ReadAll().ToList();
                            if (languages.Any())
                            {
                                var random = new Random();
                                employee.LanguageId = languages[random.Next(languages.Count)].Id;
                            }
                            else
                            {
                                employee.LanguageId = 1; // Язык по умолчанию
                            }
                        }

                        // Проверяем, существует ли уже сотрудник с таким ID
                        var existing = _iTEmployeeRepository.ReadById(employee.Id);
                        if (existing == null)
                        {
                            _iTEmployeeRepository.Add(employee);
                            Console.WriteLine($"Добавлен: {employee.FullName} (ID: {employee.Id}, LanguageId: {employee.LanguageId})");
                            successCount++;
                        }
                        else
                        {
                            Console.WriteLine($"Сотрудник с ID {employee.Id} уже существует - пропуск");
                            skippedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при добавлении {employee.FullName}: {ex.Message}");
                        skippedCount++;
                    }
                }

                var allEmployees = _iTEmployeeRepository.ReadAll();
                Console.WriteLine($"\n=== РЕЗУЛЬТАТ ИМПОРТА ===");
                Console.WriteLine($"Успешно добавлено: {successCount} сотрудников");
                Console.WriteLine($"Пропущено: {skippedCount} записей");
                Console.WriteLine($"Всего в базе: {allEmployees.Count()} сотрудников");

                if (successCount > 0)
                {
                    Console.WriteLine($"\nДобавленные сотрудники:");
                    foreach (var emp in allEmployees)
                    {
                        var language = _languageRepository.ReadById(emp.LanguageId);
                        var languageName = language?.Name ?? "Неизвестно";
                        Console.WriteLine($"  ID: {emp.Id} | {emp.FullName} | {emp.Position} | {emp.Department} | Язык: {languageName}");
                    }
                }

                // Показываем статистику по языкам
                ShowLanguageStatistics(_iTEmployeeRepository, _languageRepository);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");

                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        private static void CreateTablesIfNotExists(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                // Создаем таблицу Languages
                var createLanguagesTableSql = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Languages' AND xtype='U')
                    CREATE TABLE Languages (
                        Id INT PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        Description NVARCHAR(500) NULL
                    )";

                connection.Execute(createLanguagesTableSql);
                Console.WriteLine("Таблица Languages создана или уже существует");

                // Создаем таблицу ITEmployee с внешним ключом
                var createEmployeeTableSql = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ITEmployee' AND xtype='U')
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
                Console.WriteLine("Таблица ITEmployee создана или уже существует");

                // Добавляем тестовые языки программирования
                SeedLanguages(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании таблиц: {ex.Message}");
                throw;
            }
        }

        private static void SeedLanguages(SqlConnection connection)
        {
            // Проверяем, есть ли уже языки в таблице
            var count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Languages");

            if (count == 0)
            {
                var languages = new[]
                {
                    new { Id = 1, Name = "C#", Description = "Язык программирования от Microsoft" },
                    new { Id = 2, Name = "Java", Description = "Популярный кроссплатформенный язык" },
                    new { Id = 3, Name = "Python", Description = "Язык для Data Science и веб-разработки" },
                    new { Id = 4, Name = "JavaScript", Description = "Язык для веб-разработки" },
                    new { Id = 5, Name = "TypeScript", Description = "Типизированный JavaScript" },
                    new { Id = 6, Name = "Go", Description = "Язык от Google для системного программирования" },
                    new { Id = 7, Name = "SQL", Description = "Язык для работы с базами данных" },
                    new { Id = 8, Name = "C++", Description = "Высокопроизводительный системный язык" },
                    new { Id = 9, Name = "PHP", Description = "Язык для веб-разработки" },
                    new { Id = 10, Name = "Ruby", Description = "Динамический язык программирования" }
                };

                foreach (var lang in languages)
                {
                    connection.Execute(
                        "INSERT INTO Languages (Id, Name, Description) VALUES (@Id, @Name, @Description)",
                        lang);
                }

                Console.WriteLine($"Добавлено {languages.Length} языков программирования");

                // Показываем добавленные языки
                Console.WriteLine("\nДоступные языки программирования:");
                var addedLanguages = connection.Query<Language>("SELECT * FROM Languages ORDER BY Id");
                foreach (var lang in addedLanguages)
                {
                    Console.WriteLine($"  ID: {lang.Id} | {lang.Name} | {lang.Description}");
                }
            }
            else
            {
                Console.WriteLine($"В таблице Languages уже есть {count} записей");
            }
        }

        private static int GetNextAvailableId(string connectionString, string tableName)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                // Проверяем, существует ли таблица и есть ли в ней данные
                var tableExists = connection.ExecuteScalar<int?>(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName",
                    new { TableName = tableName });

                if (tableExists > 0)
                {
                    var maxId = connection.ExecuteScalar<int?>($"SELECT MAX(Id) FROM {tableName}");
                    return (maxId ?? 0) + 1;
                }

                return 1; // Начинаем с 1 если таблица пустая
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении следующего ID для таблицы {tableName}: {ex.Message}");
                return 1;
            }
        }

        private static void ShowLanguageStatistics(EntityRepository<ITEmployee> employeeRepo, EntityRepository<Language> languageRepo)
        {
            var employees = employeeRepo.ReadAll().ToList();
            var languages = languageRepo.ReadAll().ToList();

            Console.WriteLine($"\n=== СТАТИСТИКА ПО ЯЗЫКАМ ПРОГРАММИРОВАНИЯ ===");

            var languageStats = employees
                .GroupBy(e => e.LanguageId)
                .Select(g => new
                {
                    LanguageId = g.Key,
                    Count = g.Count(),
                    Language = languages.FirstOrDefault(l => l.Id == g.Key)?.Name ?? "Неизвестно"
                })
                .OrderByDescending(x => x.Count);

            foreach (var stat in languageStats)
            {
                Console.WriteLine($"  {stat.Language}: {stat.Count} сотрудников");
            }

            var mostPopular = languageStats.FirstOrDefault();
            if (mostPopular != null)
            {
                Console.WriteLine($"\nСамый популярный язык: {mostPopular.Language} ({mostPopular.Count} сотрудников)");
            }
        }

        private static void CreateTestJsonFile(string filePath)
        {
            var testData = new List<ITEmployee>
            {
                new ITEmployee(1, "Иванов Иван Иванович", Position.Middle, Department.Разработчик, 150000, 3, 1),
                new ITEmployee(2, "Петров Петр Петрович", Position.Senior, Department.Аналитик, 200000, 7, 3),
                new ITEmployee(3, "Сидорова Анна Сергеевна", Position.Junior, Department.Разработчик, 80000, 1, 2),
                new ITEmployee(4, "Козлов Дмитрий Владимирович", Position.Middle, Department.Кибербезопасность, 170000, 4, 4),
                new ITEmployee(5, "Никитина Ольга Александровна", Position.Senior, Department.База_данных, 190000, 8, 7)
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(testData, options);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Создан тестовый JSON файл: {filePath}");
        }
    }
}