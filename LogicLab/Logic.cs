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

