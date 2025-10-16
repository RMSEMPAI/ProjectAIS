using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.Json;

namespace LogicLib
{
    public class Logic
    {
        private List<ITEmployee> employees;
        private string data_path;
        public int nextId = 0;
        private FileSystemWatcher watcher;
        public DateTime LastSynchronizationDate { get; private set; }
        /// <summary>
        /// Конструктор для записи данных
        /// </summary>
        public Logic()
        {
            data_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "data.json");
            if (File.Exists(data_path))
                employees = JsonSerializer.Deserialize<List<ITEmployee>>(File.ReadAllText(data_path)) ?? new List<ITEmployee>();
            else
            {
                employees = new List<ITEmployee>();
                File.WriteAllText(data_path, "[]");
            }
            nextId = employees.Select(employees => employees.Id).Max() + 1;

            LastSynchronizationDate = DateTime.Now;

            watcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "data.json");
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            watcher.Changed += new FileSystemEventHandler(LoadData);
            watcher.Created += new FileSystemEventHandler(LoadData);
            watcher.EnableRaisingEvents = true;

        }
        /// <summary>
        /// Метод, читающий даные
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void LoadData(object s, FileSystemEventArgs e)
        {
            if (File.Exists(data_path))
                employees = JsonSerializer.Deserialize<List<ITEmployee>>(File.ReadAllText(data_path)) ?? new List<ITEmployee>();
            else
            {
                employees = new List<ITEmployee>();
                File.WriteAllText(data_path, "[]");
            }
            nextId = employees.Select(employees => employees.Id).Max() + 1;
            LastSynchronizationDate = DateTime.Now;
        }

        /// <summary>
        /// Метод для получения сотрудников по отделу
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public List<ITEmployee> GetEmployeesByDepartment(Department department)
        {
            return GetAllEmployees().Where(e => e.Department == department).ToList();
        }

        /// <summary>
        /// Метод для получения сотрудников по Проф.деятельности
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public List<ITEmployee> GetEmployeesByPosition(Position position)
        {
            return GetAllEmployees().Where(e => e.Position == position).ToList();
        }

        /// <summary>
        /// Метод для добавления сотрудника
        /// </summary>
        /// <param name="employee"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddEmployee(string fullname, Position position, Department department, decimal salary, int experienceyears)
        {
            if (fullname == string.Empty || position == 0 || department == 0 || salary == 0m || experienceyears == 0)
                throw new ArgumentNullException();
            else
            {
                ITEmployee employee = new ITEmployee(nextId, fullname, position, department, salary, experienceyears);
                nextId++;
                employees.Add(employee);
            }
            SaveData();
        }
        /// <summary>
        /// Метод для удаления сотрудника
        /// </summary>
        /// <param name="number"></param>
        public void RemoveEmployee(int id)
        {
            var employeeToRemove = employees.FirstOrDefault(x => x.Id == id);
            if (employeeToRemove != null)
            {
                employees.Remove(employeeToRemove);
            }
            SaveData();
        }

        /// <summary>
        /// Метод для изменения сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        public void UpdateEmployee(int id, ITEmployee employee)
        {
            var existingEmployee = employees.FirstOrDefault(e => e.Id == id);
            if (existingEmployee != null)
            {
                // Обновляем свойства существующего объекта
                existingEmployee.FullName = employee.FullName;
                existingEmployee.Position = employee.Position;
                existingEmployee.Department = employee.Department;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.ExperienceYears = employee.ExperienceYears;
            }
            SaveData();
        }

        /// <summary>
        /// Метод, возвращающий всех сотрудников
        /// </summary>
        /// <returns></returns>
        public List<ITEmployee> GetAllEmployees()
        {
            List<ITEmployee> finallist = new List<ITEmployee>();
            foreach (var employee in employees)
            {
                ITEmployee employeelist = new ITEmployee()
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    Position = employee.Position,
                    Department = employee.Department,
                    Salary = employee.Salary,
                    ExperienceYears = employee.ExperienceYears

                };
                finallist.Add(employeelist);
            }
            return finallist;
        }

        /// <summary>
        /// Метод для изменения отдела сотрудника
        /// </summary>
        /// <param name="department"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool UpdateDepartmentEmployee(Department department, ITEmployee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            // Поиск сотрудника по ID
            var existingEmployee = employees.FirstOrDefault(e => e.Id == employee.Id);

            if (existingEmployee != null)
            {
                // Изменение отдела
                existingEmployee.Department = department;
                SaveData();
                return true; // Успешно
            }

            return false; // Если сотрудник не найден
        }
        /// <summary>
        /// Метод проверки пригодности на повышение
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool IsPromoteEmployeeBasedOnExperience(ITEmployee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            if (employee.ExperienceYears >= 5 && employee.Position != Position.Senior)
                return true;
            else if (employee.ExperienceYears >= 3 && employee.Position == Position.Junior)
                return true;
            else if (employee.ExperienceYears >= 2 && employee.Position == Position.Middle)
                return true;
            return false;

        }
        /// <summary>
        /// Метод, выдающий список сотрудников на повышение
        /// </summary>
        /// <returns></returns>
        public List<ITEmployee> GetPromoteEmployees()
        {
            return GetAllEmployees().Where(x => IsPromoteEmployeeBasedOnExperience(x)).ToList();
        }

        /// <summary>
        /// Метод повышения сотрудника и его зарплаты для winform
        /// </summary>
        /// <param name="id"></param>
        public void PromoteEmployeeBasedOnExperience(int id)
        {
            PromoteEmployeeBasedOnExperience(GetEmployeeById(id));
        }

        /// <summary>
        /// Метод повышения сотрудника и его зарпалты
        /// </summary>
        /// <param name="employee"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void PromoteEmployeeBasedOnExperience(ITEmployee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            // Сохраняем исходную позицию для проверки изменений
            var originalPosition = employee.Position;

            // Повышение на основе опыта
            if (employee.ExperienceYears >= 5 && employee.Position != Position.Senior)
            {
                employee.Position = Position.Senior;
                employee.Salary *= 1.25m; // +25% к зарплате
            }
            else if (employee.ExperienceYears >= 3 && employee.Position == Position.Junior)
            {
                employee.Position = Position.Middle;
                employee.Salary *= 1.15m; // +15% к зарплате
            }
            else if (employee.ExperienceYears >= 2 && employee.Position == Position.Middle)
            {
                // Middle может получить повышение зарплаты без смены позиции
                employee.Salary *= 1.10m; // +10% к зарплате
            }
            SaveData();
        }

        /// <summary>
        /// Метод, возвращающий id специалиста
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ITEmployee GetEmployeeById(int id)
        {
            return employees.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Метод, сохраняющий данные
        /// </summary>
        public void SaveData()
        {
            File.WriteAllText(data_path, JsonSerializer.Serialize(employees));
        }
    }
}

