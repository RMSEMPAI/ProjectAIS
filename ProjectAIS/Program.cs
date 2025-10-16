using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLib;

namespace ConsoleApp1
{
    internal class Program
    {
        private static List<string> all_positions = Enum.GetNames(typeof(Position)).ToList();
        private static List<string> all_departments = Enum.GetNames(typeof(Department)).ToList();
        static void Main(string[] args)
        {
            Logic logic = new Logic();

            string command;

            do
            {
                Console.WriteLine("\nВведите команду: \n1.Добавить, \n2.Удалить, \n3.Изменить, \n4.Список, \n5.Отображение по специальностям, \n6.Отображение по отделу\n7.сотрудники на повышение \n8.сохранить и выход");
                command = Console.ReadLine().ToLower();

                switch (command)
                {
                    case "1":
                        AddEmployee(logic);
                        break;
                    case "2":
                        RemoveEmployee(logic);
                        break;
                    case "3":
                        UpdateEmployee(logic);
                        break;
                    case "4":
                        PrintEmployeeList(logic);
                        break;
                    case "5":
                        PrintEmployeeByPosition(logic);
                        break;
                    case "6":
                        PrintEmployeeByDepartment(logic);
                        break;
                    case "7":
                        PromoteEmployeeInProgram(logic);
                        break;
                    case "8":
                        logic.SaveData();
                        break;
                }
            } while (command != "exit");

        }
        /// <summary>
        /// Функция для вывода списка специалистов
        /// </summary>
        /// <param name="logic"></param>
        public static void PrintEmployeeList(Logic logic)
        {
            Console.WriteLine();
            foreach (var i in logic.GetAllEmployees())
                Console.WriteLine($"Id:{i.Id} ФИО:{i.FullName};  Отдел:{i.Department}; Позиция:{i.Position}; Опыт:{i.ExperienceYears} лет; Зарплата:{i.Salary}");
            Console.WriteLine();
        }
        /// <summary>
        /// Функция, выдающая список по позициям
        /// </summary>
        /// <param name="logic"></param>
        public static void PrintEmployeeByPosition(Logic logic)
        {
            Console.WriteLine();
            Console.WriteLine($"Введите позицию из списка [{string.Join(", ", all_positions)}]");
            string pos = Console.ReadLine();
            if (!Enum.TryParse<Position>(pos, out Position res))
            {
                Console.WriteLine("такой позиции не существует");
                return;
            }
            Console.WriteLine();
            foreach (var i in logic.GetEmployeesByPosition(res))
                Console.WriteLine($"Id:{i.Id} ФИО:{i.FullName};  Отдел:{i.Department}; Позиция:{i.Position}; Опыт:{i.ExperienceYears} лет; Зарплата:{i.Salary}");
            Console.WriteLine();
        }
        /// <summary>
        /// Функция, выдающая список по отделам
        /// </summary>
        /// <param name="logic"></param>
        public static void PrintEmployeeByDepartment(Logic logic)
        {
            Console.WriteLine();
            Console.WriteLine($"Введите отдел из списка [{string.Join(", ", all_departments)}]");
            string pos = Console.ReadLine();
            if (!Enum.TryParse<Department>(pos, out Department res))
            {
                Console.WriteLine("такого отдела не существует");
                return;
            }
            Console.WriteLine();
            foreach (var i in logic.GetEmployeesByDepartment(res))
                Console.WriteLine($"Id:{i.Id} ФИО:{i.FullName};  Отдел:{i.Department}; Позиция:{i.Position}; Опыт:{i.ExperienceYears} лет; Зарплата:{i.Salary}");
            Console.WriteLine();
        }

        /// <summary>
        /// Функция для добавления сотрудника
        /// </summary>
        /// <param name="logic"></param>
        static void AddEmployee(Logic logic)
        {
            Console.WriteLine("Введите ФИО специалиста:");
            string fullname = Console.ReadLine();
            Console.WriteLine($"Введите уровень развития из списка [{string.Join(", ", all_positions)}]:");
            string position = Console.ReadLine();
            Position position1;
            if (Enum.IsDefined(typeof(Position), position))
            {
                position1 = (Position)Enum.Parse(typeof(Position), position);
            }
            else
            {
                Console.WriteLine("Такого уровня развития не существует!!!");
                Console.WriteLine($"Доступные уровни развития: {string.Join(", ", all_positions)}");
                return;
            }

            Console.WriteLine($"Введите отдел специалиста из списка [{string.Join(", ", all_departments)}]:");
            string department = Console.ReadLine();
            Department department1;

            if (Enum.IsDefined(typeof(Department), department))
            {
                department1 = (Department)Enum.Parse(typeof(Department), department);
            }
            else
            {
                Console.WriteLine("Такого отдела не существует!!!");
                Console.WriteLine($"Доступные отделы: {string.Join(", ", all_departments)}");
                return;
            }
            decimal salary;
            while (true)
            {
                Console.WriteLine("Введите зарплату специалиста:");
                if (decimal.TryParse(Console.ReadLine(), out salary) && salary > 0)
                {
                    Console.WriteLine($"Зарплата принята: {salary} руб.");
                    break;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите положительное число (например: 50000 или 75000.50)");
                }
            }
            Console.WriteLine("Введите опыт работы сотрудника:");
            if (int.TryParse(Console.ReadLine(), out int experienceyears))
            {
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Введите опыт работы числом!!!");
            }
            try
            { logic.AddEmployee(fullname, position1, department1, salary, experienceyears); }
            catch
            {
                Console.WriteLine("Ошибка!");
                return;
            }
            Console.WriteLine("Специалист добавлен");
        }
        /// <summary>
        /// Функция для удаления специалиста
        /// </summary>
        /// <param name="logic"></param>
        static void RemoveEmployee(Logic logic)
        {
            if (logic.GetAllEmployees().Count == 0)
            {
                Console.WriteLine("Список специалистов пуст!");
                return;
            }
            else
            {
                Console.Write("Введите id специалиста для удаления: \n");
                int id = Convert.ToInt32(Console.ReadLine());

                try { logic.RemoveEmployee(id); }
                catch
                {
                    Console.WriteLine("Ошибка!");
                    return;
                }
                Console.WriteLine("Специалист удален.");
            }
        }
        /// <summary>
        /// Функция для обновления данных 
        /// </summary>
        /// <param name="logic"></param>
        public static void UpdateEmployee(Logic logic)
        {
            Console.Write("Введите ID специалиста для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка: Введите корректный ID!");
                return;
            }
            var existing = logic.GetEmployeeById(id);
            if (existing == null)
            {
                Console.WriteLine($"Сотрудник с ID {id} не найден!");
                return;
            }

            Position position;
            while (true)
            {
                Console.WriteLine($"Введите уровень развития из списка [{string.Join(", ", all_positions)}]: ");
                string posInput = Console.ReadLine();

                if (Enum.TryParse<Position>(posInput, true, out position) &&
                    Enum.IsDefined(typeof(Position), position))
                {
                    break;
                }
                Console.WriteLine("Неверный уровень развития! Попробуйте снова.");
            }
            Console.Write("Введите зарплату: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
            {
                Console.WriteLine("Ошибка ввода зарплаты!");
                return;
            }

            Console.Write("Введите опыт работы (лет): ");
            if (!int.TryParse(Console.ReadLine(), out int experience))
            {
                Console.WriteLine("Ошибка ввода опыта!");
                return;
            }

            Department department;
            while (true)
            {
                Console.Write($"Введите отдел из списка [{string.Join(", ", all_departments)}]: ");
                string deptInput = Console.ReadLine();

                if (Enum.TryParse<Department>(deptInput, true, out department) &&
                    Enum.IsDefined(typeof(Department), department))
                {
                    break;
                }
                Console.WriteLine("Неверный отдел! Попробуйте снова.");
            }

            var updatedEmployee = new ITEmployee(0, existing.FullName, position, department, salary, experience);

            try
            {
                logic.UpdateEmployee(id, updatedEmployee);
                Console.WriteLine("Данные специалиста успешно обновлены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении: {ex.Message}");
            }
        }
        /// <summary>
        /// Функция для повышения
        /// </summary>
        /// <param name="logic"></param>
        public static void PromoteEmployeeInProgram(Logic logic)
        {

            // Получаем список всех сотрудников
            var allEmployees = logic.GetAllEmployees();
            if (!allEmployees.Any(x => logic.IsPromoteEmployeeBasedOnExperience(x)))
            {
                Console.WriteLine("Нет сотрудников для повышения!");
                return;
            }

            // Показываем список сотрудников
            Console.WriteLine("\nСписок сотрудников:");
            foreach (var emp in logic.GetPromoteEmployees())
            {
                Console.WriteLine($"ID: {emp.Id} | {emp.FullName} | {emp.Position} | Опыт: {emp.ExperienceYears} лет | Зарплата: {emp.Salary} руб.");
            }

            // Выбор сотрудника
            Console.Write("\nВведите ID сотрудника для повышения: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("Ошибка: Введите корректный ID!");
                return;
            }

            // Находим сотрудника
            var employee = logic.GetEmployeeById(employeeId);
            if (employee == null)
            {
                Console.WriteLine($"Сотрудник с ID {employeeId} не найден!");
                return;
            }

            // Показываем текущие данные
            Console.WriteLine($"\nТекущие данные сотрудника:");
            Console.WriteLine($"Имя: {employee.FullName}");
            Console.WriteLine($"Должность: {employee.Position}");
            Console.WriteLine($"Опыт: {employee.ExperienceYears} лет");
            Console.WriteLine($"Зарплата: {employee.Salary} руб.");

            // Проверяем возможность повышения
            var originalPosition = employee.Position;
            var originalSalary = employee.Salary;

            try
            {
                // Применяем повышение
                logic.PromoteEmployeeBasedOnExperience(employee);

                // Проверяем, было ли повышение
                if (employee.Position != originalPosition || employee.Salary != originalSalary)
                {
                    Console.WriteLine($"Новая должность: {employee.Position}");
                    Console.WriteLine($"Новая зарплата: {employee.Salary} руб.");
                    Console.WriteLine($"Повышение зарплаты: {employee.Salary - originalSalary} руб.");

                    // Сохраняем изменения
                    logic.UpdateEmployee(employee.Id, employee);
                }
                else
                {
                    Console.WriteLine("\nПовышение не предусмотрено для текущего опыта и должности.");
                    Console.WriteLine("Условия для повышения:");
                    Console.WriteLine("С Junior на Middle: от 3 лет опыта");
                    Console.WriteLine("С Middle на Senior: от 5 лет опыта");
                    Console.WriteLine("Middle: +10% зарплаты от 2 лет опыта");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при повышении: {ex.Message}");
            }
        }
        /// <summary>
        /// Функция для изменения отдела
        /// </summary>
        /// <param name="logic"></param>
        public static void ChangeEmployeeDepartment(Logic logic)
        {

            // Получаем список всех сотрудников
            var allEmployees = logic.GetAllEmployees();
            if (!allEmployees.Any())
            {
                Console.WriteLine("Нет сотрудников в системе!");
                return;
            }

            // Показываем список сотрудников
            Console.WriteLine("\nСписок сотрудников:");
            foreach (var emp in allEmployees)
            {
                Console.WriteLine($"ID: {emp.Id} | {emp.FullName} | {emp.Department} | {emp.Position}");
            }

            // Выбор сотрудника
            Console.Write("\nВведите ID сотрудника для изменения отдела: ");
            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("Ошибка: Введите корректный ID!");
                return;
            }

            // Находим сотрудника
            var employee = logic.GetEmployeeById(employeeId);
            if (employee == null)
            {
                Console.WriteLine($"Сотрудник с ID {employeeId} не найден!");
                return;
            }

            Console.WriteLine($"\nВыбран сотрудник: {employee.FullName}");
            Console.WriteLine($"Текущий отдел: {employee.Department}");

            // Выбор нового отдела
            Department newDepartment;
            while (true)
            {
                Console.WriteLine("\nДоступные отделы:");
                var departments = Enum.GetValues(typeof(Department));
                foreach (Department dept in departments)
                {
                    Console.WriteLine($"- {dept}");
                }

                Console.Write("Введите название нового отдела: ");
                string departmentInput = Console.ReadLine();

                if (Enum.TryParse<Department>(departmentInput, true, out newDepartment) &&
                    Enum.IsDefined(typeof(Department), newDepartment))
                {
                    break;
                }
                Console.WriteLine("Неверное название отдела! Попробуйте снова.");
            }

            // Проверяем, не совпадает ли текущий отдел с новым
            if (employee.Department == newDepartment)
            {
                Console.WriteLine($"Сотрудник уже работает в отделе {newDepartment}!");
                return;
            }

            // Выполняем изменение отдела
            try
            {
                bool success = logic.UpdateDepartmentEmployee(newDepartment, employee);

                if (success)
                {
                    Console.WriteLine($"Сотрудник: {employee.FullName}");
                    Console.WriteLine($"Новый отдел: {newDepartment}");

                    // Обновляем основные данные сотрудника
                    logic.UpdateEmployee(employee.Id, employee);
                }
                else
                {
                    Console.WriteLine("Не удалось изменить отдел сотрудника.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при изменении отдела: {ex.Message}");
            }
        }
    }
}
