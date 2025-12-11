using DataAccessLayer;
using LogicLab;
using System.Text.Json.Serialization;
using LogicLab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLib
{
    

        public class ITEmployee : IDomainObject
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public Position Position { get; set; }
            public Department Department { get; set; }
            public decimal Salary { get; set; }
            public int ExperienceYears { get; set; }

            public int LanguageId { get; set; }

            public virtual Language Language { get; set; }

            public ITEmployee() { }

            public ITEmployee(int id, string fullName, Position position, Department department,
                             decimal salary, int experienceYears, int languageId)
            {
                Id = id;
                FullName = fullName;
                Position = position;
                Department = department;
                Salary = salary;
                ExperienceYears = experienceYears;
                LanguageId = languageId;
            }
        }
    

    /// <summary>
    /// Отделы
    /// </summary>
    public enum Department
    {
        Unknown = 0,
        Аналитик,
        Кибербезопасность,
        Информационная_структура,
        Разработчик,
        ERP_платформы,
        База_данных
    }
    /// <summary>
    /// Проф. развитие
    /// </summary>
    public enum Position
    {
        Unknown = 0,
        Junior,
        Middle,
        Senior
    }
}
