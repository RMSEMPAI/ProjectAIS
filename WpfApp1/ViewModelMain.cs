using LogicLib;
using Shared;
using System;
using Newtonsoft;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            commandAdd = new RelayCommand(Add, CheckAdd);
            commandDelete = new RelayCommand(Delete,CheckSelectedNull);
            commandPromote = new RelayCommand(OkPromote,CheckPromote);
            commandUpdate = new RelayCommand(UpdateEmployee,CheckUpdate);
            commandImport = new RelayCommand(CreateImport, null);
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
                showAll = false;
                showOnPosition = true;
                showOnDepartment = false;
                UpdateListEmployees();
                OnPropertyChanged("PositionChoose");
            } }
        private int positionChoose;

        public int DepartmentChoose { get => departmentChoose; set {
                departmentChoose = value;
                showOnDepartment = true;
                showAll = false;
                showOnPosition= false;
                UpdateListEmployees();
                OnPropertyChanged("DepartmentChoose");
            } }
        private int departmentChoose;

        public int PositionFilter { get => positionFilter; set {
                positionFilter = value;
                OnPropertyChanged("PositionFilter");
            } }
        private int positionFilter;

        public int DepartmentFilter { get => departmentFilter; set {
                departmentFilter = value;
                OnPropertyChanged("DepartmentFilter");
            } }
        private int departmentFilter;

        public string Name { get => name; set {
                name = value;
                commandAdd.RaiseCanExecuteChanged();
                commandUpdate.RaiseCanExecuteChanged();
                OnPropertyChanged("Name");
            } }
        private string name;

        public int PositionEmployee { get => positionEmployee; set {
                positionEmployee = value;
                commandAdd.RaiseCanExecuteChanged();
                commandUpdate.RaiseCanExecuteChanged();
                OnPropertyChanged("PositionEmployee");
            } }
        private int positionEmployee;

        public int DepartmentEmployee { get => departmentEmployee; set {
                departmentEmployee = value;
                commandAdd.RaiseCanExecuteChanged();
                commandUpdate.RaiseCanExecuteChanged();
                OnPropertyChanged("DepartmentEmployee");
            } }
        private int departmentEmployee;

        public int Salary { get => salary; set {
                salary = value;
                commandAdd.RaiseCanExecuteChanged();
                commandUpdate.RaiseCanExecuteChanged();
                OnPropertyChanged("Salary");
            } }
        private int salary;

        public int Expirience {  get => expirience; set {
                expirience = value;
                commandAdd.RaiseCanExecuteChanged();
                commandUpdate.RaiseCanExecuteChanged();
                OnPropertyChanged("Expirience");
            } }
        private int expirience;

        public string Language { get => language; set {
                language = value;
                commandAdd.RaiseCanExecuteChanged();
                commandUpdate.RaiseCanExecuteChanged();
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

        public RelayCommand commandAdd { get; private set; }
        public void Add()
        {
            if (CheckAdd())model.AddEmployee(new ITEmployee { FullName = Name, Position = (Position)PositionEmployee, Department = (Department)DepartmentEmployee, Salary = salary, ExperienceYears = Expirience },Language.Replace("System.Windows.Controls.ComboBoxItem: ",""));
            UpdateListEmployees();
        }

        
        public ITEmployee SelectedEmployee { get => selectedEmployee; set {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
                commandDelete.RaiseCanExecuteChanged();
                commandAdd.RaiseCanExecuteChanged();
                commandPromote.RaiseCanExecuteChanged();
                commandUpdate.RaiseCanExecuteChanged();
            } }
        private ITEmployee selectedEmployee;
        

        public RelayCommand commandDelete { get; private set; }
        public void Delete()
        {
            model.DeleteEmployee(SelectedEmployee.Id);
            UpdateListEmployees();
        }


        public RelayCommand commandPromote { get; private set; }

        public void OkPromote()
        {
            if (SelectedEmployee == null) return;
            if (model.IsPromoteEmployeeBasedOnExperience(SelectedEmployee))
                model.PromoteEmployeeBasedOnExperience(SelectedEmployee);
            UpdateListEmployees();
        }

        public RelayCommand commandUpdate { get; private set; }

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

        public RelayCommand commandImport { get; private set; }


        public bool CheckSelectedNull()
        {
            return selectedEmployee != null && model.GetEmployeeById(selectedEmployee.Id) != null;
        }
        public bool CheckPromote()
        {
            return CheckSelectedNull() && model.IsPromoteEmployeeBasedOnExperience(selectedEmployee);
        }
        public bool CheckUpdate()
        {
            return CheckSelectedNull() && CheckAdd();
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

        private void CreateImport()
        {
            try
            {

                var allEmployees = model.GetAllEmployees();

                var debugInfo = $"DEBUG:\n" +
                               $"Всего сотрудников: {employees.Count}\n" +
                               $"Сотрудники\n" +
                               $"  Позиция: {(Position)(PositionFilter - 1)}\n" +
                               $"  Отдел: {(Department)(DepartmentFilter - 1)}\n\n";


                MessageBox.Show(debugInfo, "Отладка", MessageBoxButton.OK, MessageBoxImage.Information);

                var filteredEmployee = new List<ITEmployee>();
                foreach(var i in allEmployees)
                {
                    if (DepartmentFilter == 0)
                    {
                        if (PositionFilter == 0)
                        {
                            filteredEmployee.Add(i);
                        }
                        if (i.Position == (Position)(PositionFilter-1))
                        {
                            filteredEmployee.Add(i);
                        } 
                    }
                    else if (PositionFilter == 0)
                    {
                        if (DepartmentFilter == 0)
                        {
                            filteredEmployee.Add(i);
                        }
                        if (i.Department == (Department)(DepartmentFilter-1))
                            filteredEmployee.Add(i);
                    }
                    else
                    {
                        if (i.Position == (Position)(PositionFilter - 1))
                        if (i.Department == (Department)(DepartmentFilter - 1))
                        filteredEmployee.Add(i); 
                    }
                }
                    if (filteredEmployee.Count() == 0)
                    {
                        MessageBox.Show($"Сотрудников с такой позицией:{(Position)(PositionFilter-1)} и с таким отделом: {(Department)(DepartmentFilter-1)} не найдено",
                            "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string jsonFilePath = System.IO.Path.Combine(desktopPath, $"report.json");
                    string csvFilePath = System.IO.Path.Combine(desktopPath, $"report.csv");

                    CreateJsonReport(filteredEmployee, jsonFilePath);

                    CreateCsvReport(filteredEmployee, csvFilePath);

                    var message = $"Отчет успешно создан!\n\n" +
                                  $"Файлы сохранены на рабочем столе:\n" +
                                  $"📄 {System.IO.Path.GetFileName(jsonFilePath)}\n" +
                                  $"📄 {System.IO.Path.GetFileName(csvFilePath)}";

                    MessageBox.Show(message, "Отчет создан", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateJsonReport(List<ITEmployee> employees, string filePath)
        {
            try
            {
                var report = new
                {
                    GeneratedAt = DateTime.Now,
                    TotalITEmployee = employees.Count,
                    ITEmployee = employees.Select(e => new
                    {
                        e.Id,
                        e.FullName,
                        e.Position,
                        e.Department,
                        e.Salary,
                        e.ExperienceYears,
                        Language = e.Language.Name
                    }).ToList()
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(report, Newtonsoft.Json.Formatting.Indented);

                System.IO.File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка создания JSON отчета: {ex.Message}", ex);
            }
        }

        private void CreateCsvReport(List<ITEmployee> employees, string filePath)
        {
            try
            {
                var csv = new System.Text.StringBuilder();

                csv.AppendLine("ID,ФИО,Уровень Проф.подготовки,Отдел,Зарплата,Язык программирования");

                foreach (var employee in employees)
                {
                    csv.AppendLine($"{employee.Id},\"{employee.FullName}\",{employee.Position}\",{employee.Department},\"{employee.Salary}\",{employee.ExperienceYears}\",{employee.Language.Name}");
                }

                System.IO.File.WriteAllText(filePath, csv.ToString(), System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка создания CSV отчета: {ex.Message}", ex);
            }
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
