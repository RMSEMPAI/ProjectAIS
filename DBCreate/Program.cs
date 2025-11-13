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
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var _mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lab1Data");

                

                var jsonData = File.ReadAllText("C:\\Users\\stepa\\AppData\\Roaming\\data.json");
                var data = JsonSerializer.Deserialize<List<ITEmployee>>(jsonData);

                if (data == null || !data.Any())
                {
                    Console.WriteLine("Нет данных для импорта");
                    return;
                }

                var connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\stepa\\source\\repos\\Khomkolova\\ProjectAIS\\DataAccessLayer\\Database1.mdf;Integrated Security=True;";

                // Сначала создадим таблицу через Dapper с правильным именем
                CreateTableIfNotExists(connectionString);

                var optionsBuilder = new DbContextOptionsBuilder<ITEmployeeContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using var context = new ITEmployeeContext(optionsBuilder.Options);

                // Создаем БД если не существует
                context.Database.EnsureCreated();

                var _iTEmployeeRepository = new EntityRepository<ITEmployee>(context);

                Console.WriteLine($"Найдено {data.Count} записей в JSON файле");

                // Получаем следующий доступный ID
                int nextId = GetNextAvailableId(connectionString);

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
                            employee.Id = nextId++;
                        }

                        // Проверяем, существует ли уже сотрудник с таким ID
                        var existing = _iTEmployeeRepository.ReadById(employee.Id);
                        if (existing == null)
                        {
                            _iTEmployeeRepository.Add(employee);
                            Console.WriteLine($"Добавлен: {employee.FullName} (ID: {employee.Id})");
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
                        Console.WriteLine($"  ID: {emp.Id} | {emp.FullName} | {emp.Position} | {emp.Department}");
                    }
                }
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

        private static int GetNextAvailableId(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                // Проверяем, существует ли таблица и есть ли в ней данные
                var tableExists = connection.ExecuteScalar<int?>(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ITEmployee'");

                if (tableExists > 0)
                {
                    var maxId = connection.ExecuteScalar<int?>("SELECT MAX(Id) FROM ITEmployee");
                    return (maxId ?? 0) + 1;
                }

                return 1; // Начинаем с 1 если таблица пустая
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении следующего ID: {ex.Message}");
                return 1;
            }
        }

        private static void CreateTableIfNotExists(string connectionString)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                var createTableSql = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ITEmployee' AND xtype='U')
                    CREATE TABLE ITEmployee (
                        Id INT PRIMARY KEY,
                        FullName NVARCHAR(200) NOT NULL,
                        Position NVARCHAR(50) NOT NULL,
                        Department NVARCHAR(50) NOT NULL,
                        Salary DECIMAL(18,2) NOT NULL,
                        ExperienceYears INT NOT NULL
                    )";

                connection.Execute(createTableSql);
                Console.WriteLine("Таблица ITEmployee создана или уже существует");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании таблицы: {ex.Message}");
                throw;
            }
        }
    }
}