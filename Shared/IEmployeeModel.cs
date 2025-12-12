using DataAccessLayer;
using LogicLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IEmployeeModel: IModel
    {
        public List<ITEmployee> GetEmployeeByDepartment(Department department);
        public List<ITEmployee> GetEmployeeByPosition(Position position);
        public void AddEmployee(ITEmployee employee, string languages = "Unknow");
        public void DeleteEmployee(int id);
        public void UpdateEmployee(ITEmployee iTEmployee);
        public void UpdateEmployee(ITEmployee employee, string languages);
        public List<ITEmployee> GetAllEmployees(bool sort = false);
        public ITEmployee GetEmployeeById(int id);
        public List<Language> GetAllLanguages();

    }
}
