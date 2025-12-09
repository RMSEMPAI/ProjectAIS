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
            view.PromoteEmployeeBasedOnExperience += model.PromoteEmployeeBasedOnExperience;
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
                list = model.GetPromoteEmployees();
            }
            else
            {
                list = model.GetAllEmployees();
            }
            view.SetDataEmployees(list);
        }
    }
}
