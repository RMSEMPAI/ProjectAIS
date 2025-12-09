using Microsoft.EntityFrameworkCore;
using LogicLab;
using LogicLib;
using DataAccessLayer;
using LogicLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Shared;

namespace LogicLib
{
    public class Logic: IEmployeeModel
    {
        private IRepository<Language> _languageContext;
        private IRepository <ITEmployee> _context;
        public int nextId = 0;

        public Logic(IRepository<ITEmployee> itEmploeerRepository, IRepository<Language> languageRepository)
        {
            _context = itEmploeerRepository;
            _languageContext = languageRepository;
        }
        /// <summary>
        /// Метод, читающий даные
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>

        /// <summary>
        /// Метод для получения сотрудников по отделу
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public List<ITEmployee> GetEmployeeByDepartment(Department department)
        {
            return _context.ReadAll().Where(x => x.Department == department).ToList();
        }

        /// <summary>
        /// Метод для получения сотрудников по Проф.деятельности
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public List <ITEmployee> GetEmployeeByPosition(Position position)
        {
            return _context.ReadAll().Where(x => x.Position == position).ToList();
        }

        /// <summary>
        /// Метод для добавления сотрудника
        /// </summary>
        /// <param name="employee"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddEmployee(ITEmployee employee,string languages = "Unknow")
        {
            var language = _languageContext.ReadAll().Where(x => x.Name ==languages).FirstOrDefault();

            

            if (language == null)
            {
                throw new ArgumentException($"Язык с ID {employee.LanguageId} не существует");
            }

            if (employee.LanguageId == 0)
            {
                employee.LanguageId = language.Id;
            }

            _context.Add(employee);
        }
        /// <summary>
        /// Метод для удаления сотрудника
        /// </summary>
        /// <param name="number"></param>
        public void DeleteEmployee(int id)
        {
            _context.Delete(id);
        }

        /// <summary>
        /// Метод для изменения сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        public void UpdateEmployee(ITEmployee iTEmployee)
        {
            _context.Update(iTEmployee);
        }
        public void UpdateEmployee(ITEmployee employee, string languages)
        {
            var language = _languageContext.ReadAll().Where(x => x.Name == languages).FirstOrDefault();

            if (language == null)
            {
                throw new ArgumentException($"Язык с ID {employee.LanguageId} не существует");
            }

            if(employee.LanguageId == 0)
            {
                employee.LanguageId = language.Id;
            }

            _context.Update(employee);
        }
        /// <summary>
        /// Метод, возвращающий всех сотрудников
        /// </summary>
        /// <returns></returns>
        public List<ITEmployee> GetAllEmployees(bool sort = false)
        {
            var employees = _context.ReadAll();
            if (sort)
                return employees.OrderBy(x => x.Department).ToList();

            return employees.ToList();
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

            var existingEmployee = _context.ReadById(employee.Id);

            if (existingEmployee != null)
            {
                var updatedEmployee = new ITEmployee
                {
                    Id = existingEmployee.Id,
                    FullName = existingEmployee.FullName,
                    Position = existingEmployee.Position,
                    Department = department,
                    Salary = existingEmployee.Salary,
                    ExperienceYears = existingEmployee.ExperienceYears
                };

                _context.Update(updatedEmployee);
                return true;
            }
            return false;
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
            return _context.ReadAll().Where(x => IsPromoteEmployeeBasedOnExperience(x)).ToList();
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

            var originalPosition = employee.Position;

            if (employee.ExperienceYears >= 5 && employee.Position != Position.Senior)
            {
                employee.Position = Position.Senior;
                employee.Salary *= 1.25m; 
            }
            else if (employee.ExperienceYears >= 3 && employee.Position == Position.Junior)
            {
                employee.Position = Position.Middle;
                employee.Salary *= 1.15m; 
            }
            else if (employee.ExperienceYears >= 2 && employee.Position == Position.Middle)
            {
                
                employee.Salary *= 1.10m; 
            }
            UpdateEmployee(employee);
            _context.SaveChanges();
        }

        /// <summary>
        /// Метод, возвращающий id специалиста
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ITEmployee GetEmployeeById(int id)
        {
            return _context.ReadById(id);
        }


        public enum SQLProvider
        {
            EF,
            Dapper
        }

        // Новые методы для работы с языками
        public List<Language> GetAllLanguages()
        {
            return _languageContext.ReadAll().ToList();
        }

        public Language GetLanguageById(int id)
        {
            return _languageContext.ReadById(id);
        }

        public List<ITEmployee> GetEmployeesByLanguage(int languageId)
        {
            return _context.ReadAll()
                .Where(e => e.LanguageId == languageId)
                .ToList();
        }
    }
}

