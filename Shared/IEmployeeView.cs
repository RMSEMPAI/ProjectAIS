using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLib;

namespace Shared
{
    public interface IEmployeeView: IView
    {
        public Func<bool,List<ITEmployee>> GetAllEmployees { get; set; }
        public Func<Department, List<ITEmployee>> GetEmployeeByDepartment {  get; set; }
        public Func<Position, List<ITEmployee>> GetEmployeeByPosition { get; set; }
        public Func<List<ITEmployee>> GetPromoteEmployee { get; set; }
        Action<ITEmployee, string> SafeEmployee { get; set; }
        Action<ITEmployee, string> OnUpdateEmployee { get; set; }
        Action<int> DeleteEmployeeByID { get; set; }
        Action<int> PromoteEmployeeBasedOnExperience { get; set; }
        Func<int,ITEmployee> GetEmployeeById { get; set; }

    }
}
