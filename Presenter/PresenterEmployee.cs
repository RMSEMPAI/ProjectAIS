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
            view.OnUpdateEmployee = model.UpdateEmployee;
            view.SafeEmployee = model.AddEmployee;
            view.DeleteEmployeeByID = model.DeleteEmployee;
            view.PromoteEmployeeBasedOnExperience = model.PromoteEmployeeBasedOnExperience;
            view.GetEmployeeById = model.GetEmployeeById;
            view.GetAllEmployees = model.GetAllEmployees;
            view.GetEmployeeByPosition = model.GetEmployeeByPosition;
            view.GetEmployeeByDepartment = model.GetEmployeeByDepartment;
            view.GetPromoteEmployee = model.GetPromoteEmployees;

            


        }
        public void Run()
        {
            view.ShowView();
        }
        public void Stop()
        {
            view.CloseView();
        }
        
        
    }
}
