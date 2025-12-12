using LogicLib;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Presenter
{
    public class PresenterEmployee
    {
        private IEmployeeModel model;
        private IEmployeeView view;
        


        public PresenterEmployee(IEmployeeModel model, IEmployeeView view)
        {
            this.model = model;
            this.view = view;
            view.RequestDataEmployees += GetDataEmployee;
            view.OnUpdateEmployee += model.UpdateEmployee;
            view.SafeEmployee += model.AddEmployee;
            view.DeleteEmployeeByID += model.DeleteEmployee;
            view.PromoteEmployeeBasedOnExperience += PromoteEmployeeBasedOnExperience;
            view.GetEmployeeByID += GetEmployeeByID;


        }
        public void Run()
        {
            view.ShowView();
        }
        public void Stop()
        {
            view.CloseView();
        }
        public void GetEmployeeByID(int id)
        {
            view.SetEmployee(model.GetEmployeeById(id));
        }
        public void GetDataEmployee(DataRequest request)
        {
            var list = new List<ITEmployee>();
            if (request.IsPosition)
            {
                list = model.GetEmployeeByPosition(request.position);
            }
            else if (request.IsDepartment)
            {
                list = model.GetEmployeeByDepartment(request.department);
            }
            else if (request.Promote)
            {
                list = GetPromoteEmployees();
            }
            else
            {
                list = model.GetAllEmployees();
            }
            view.SetDataEmployees(list);
        }

        public bool UpdateDepartmentEmployee(Department department, ITEmployee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            var existingEmployee = model.GetEmployeeById(employee.Id);

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

                model.UpdateEmployee(updatedEmployee);
                return true;
            }
            return false;
        }
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
            return model.GetAllEmployees().Where(x => IsPromoteEmployeeBasedOnExperience(x)).ToList();
        }

        /// <summary>
        /// Метод повышения сотрудника и его зарплаты для winform
        /// </summary>
        /// <param name="id"></param>
        public void PromoteEmployeeBasedOnExperience(int id)
        {
            PromoteEmployeeBasedOnExperience(model.GetEmployeeById(id));
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
            model.UpdateEmployee(employee);
        }

    }
}
