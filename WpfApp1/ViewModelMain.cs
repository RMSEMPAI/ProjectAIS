using LogicLib;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfApp1
{
    public class ViewModelMain : IViewModel
    {
        public event EventHandler? Initialized;
        public event PropertyChangedEventHandler? PropertyChanged;

        public IEmployeeModel model { get; set; }

        public ObservableCollection<ITEmployee> employees { get; set; } = new ObservableCollection<ITEmployee>();

        public ViewModelMain(IEmployeeModel model)
        {
            this.model = model;
            commandAdd = new RelayCommand(Add);
            commandDelete = new RelayCommand(Delete);
            commandPromote = new RelayCommand(OkPromote);
            commandUpdate = new RelayCommand(UpdateEmployee);
            UpdateListEmployees();
        }
        
        public bool ShowAll { get => showAll; set {
                showAll = true;
                showOnPosition = false;
                showOnDepartment = false;
                showOnPromote = false;
                UpdateListEmployees();
                OnPropertyChanged("ShowAll");
                OnPropertyChanged("ShowOnPosition");
                OnPropertyChanged("ShowOnDepartment");
                OnPropertyChanged("ShowOnPromote");
            } }
        private bool showAll = true;

        public bool ShowOnPosition {  get => showOnPosition; set {
                showOnPosition = true;
                showAll = false;
                showOnDepartment = false;
                showOnPromote = false;
                UpdateListEmployees();
                OnPropertyChanged("ShowOnPosition");
                OnPropertyChanged("ShowAll");
                OnPropertyChanged("ShowOnDepartment");
                OnPropertyChanged("ShowOnPromote");
            } }
        private bool showOnPosition;

        public bool ShowOnDepartment {  get => showOnDepartment; set {
                showOnDepartment = true;
                showOnPosition = false;
                showAll = false;
                showOnPromote = false;
                UpdateListEmployees();
                OnPropertyChanged("ShowAll");
                OnPropertyChanged("ShowOnPosition");
                OnPropertyChanged("ShowOnDepartment");
                OnPropertyChanged("ShowOnPromote");
            } }
        private bool showOnDepartment;

        public bool ShowOnPromote { get => showOnPromote; set {
                showOnPromote = true;
                showOnPosition = false;
                showAll = false;
                showOnDepartment = false;
                UpdateListEmployees();
                OnPropertyChanged("ShowAll");
                OnPropertyChanged("ShowOnPosition");
                OnPropertyChanged("ShowOnDepartment");
                OnPropertyChanged("ShowOnPromote");
            } }
        private bool showOnPromote;

        public int PositionChoose { get => positionChoose; set {
                positionChoose = value;
                UpdateListEmployees();
                OnPropertyChanged("PositionChoose");
            } }
        private int positionChoose;

        public int DepartmentChoose { get => departmentChoose; set {
                departmentChoose = value;
                UpdateListEmployees();
                OnPropertyChanged("DepartmentChoose");
            } }
        private int departmentChoose;

        public string Name { get => name; set {
                name = value;
                OnPropertyChanged("Name");
            } }
        private string name;

        public int PositionEmployee { get => positionEmployee; set {
                positionEmployee = value;
                OnPropertyChanged("PositionEmployee");
            } }
        private int positionEmployee;

        public int DepartmentEmployee { get => departmentEmployee; set {
                departmentEmployee = value;
                OnPropertyChanged("DepartmentEmployee");
            } }
        private int departmentEmployee;

        public int Salary { get => salary; set {
                salary = value;
                OnPropertyChanged("Salary");
            } }
        private int salary;

        public int Expirience {  get => expirience; set {
                expirience = value;
                OnPropertyChanged("Expirience");
            } }
        private int expirience;

        public string Language { get => language; set {
                language = value;
                OnPropertyChanged("Language");
            } }
        private string language = "Unknow";
        public void UpdateListEmployees()
        {
            var list = new List<ITEmployee>();
            if (ShowOnPosition)
            {
                list = model.GetEmployeeByPosition((Position)PositionChoose);
            }
            else if (ShowOnDepartment)
            {
                list = model.GetEmployeeByDepartment((Department)DepartmentChoose);
            }
            else if (ShowOnPromote)

            {
                list = model.GetPromoteEmployees();
            }
            else
            {
                list = model.GetAllEmployees();
            }
            employees.Clear();
            foreach (var employee in list)
            {
                employees.Add(employee);
            }
        }

        public RelayCommand commandAdd { get; set; }
        public void Add()
        {
            if (CheckAdd())model.AddEmployee(new ITEmployee { FullName = Name, Position = (Position)PositionEmployee, Department = (Department)DepartmentEmployee, Salary = salary, ExperienceYears = Expirience },Language.Replace("System.Windows.Controls.ComboBoxItem: ",""));
            UpdateListEmployees();
        }

        
        public ITEmployee SelectedEmployee { get => selectedEmployee; set {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            } }
        private ITEmployee selectedEmployee;
        

        public RelayCommand commandDelete { get; set; }
        public void Delete()
        {
            model.DeleteEmployee(SelectedEmployee.Id);
            UpdateListEmployees();
        }


        public RelayCommand commandPromote { get; set; }

        public void OkPromote()
        {
            if (SelectedEmployee == null) return;
            if (model.IsPromoteEmployeeBasedOnExperience(SelectedEmployee))
                model.PromoteEmployeeBasedOnExperience(SelectedEmployee);
            UpdateListEmployees();
        }

        public RelayCommand commandUpdate { get; set; }

        public void UpdateEmployee()
        {
            if (Name == null || string.IsNullOrWhiteSpace(Name))
            {
                return ;
            }
            if (Language == null || string.IsNullOrWhiteSpace(Language)) { return; }
            var e = model.GetEmployeeById(SelectedEmployee.Id);
            e.FullName = Name;
            e.Position = (Position)PositionEmployee;
            e.Department = (Department)DepartmentEmployee;
            e.Salary = salary;
            e.ExperienceYears = Expirience;
            model.UpdateEmployee(e,ex());
            UpdateListEmployees();
        }

        public bool CheckAdd()
        {
            if(Name == null || string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void Initialize()
        {
            Initialized?.Invoke(this, EventArgs.Empty);
        }

















































































        public string ex()
        {
            var x = Language.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            return x;
        }
    }
}
