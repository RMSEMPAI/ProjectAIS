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

        public ITEmployee(int id, string fullName, Position position, Department department, decimal salary, int experienceYears)
        {
            Id = id;
            FullName = fullName;
            this.Position = position;
            this.Department = department;
            Salary = salary;
            ExperienceYears = experienceYears;
        }
        public ITEmployee()
        {

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
