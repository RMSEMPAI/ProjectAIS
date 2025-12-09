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
        event Action<DataRequest> RequestDataEmployees;
        event Action<ITEmployee, string> SafeEmployee;
        event Action<ITEmployee, string> OnUpdateEmployee;
        event Action<int> GetEmployeeByID;
        event Action<int> DeleteEmployeeByID;
        event Action<int> PromoteEmployeeBasedOnExperience;
        public void SetEmployee(ITEmployee iTEmployee);


        public void SetDataEmployees(List<ITEmployee> iTEmployees);
    }
}
