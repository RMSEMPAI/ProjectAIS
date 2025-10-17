using LogicLib;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Laba1
{
    public partial class UpdateEmployee : Form
    {
        private Action<ITEmployee> action;
        private List<string> all_positions;
        private List<string> all_departments;
        public UpdateEmployee(EventForm eventForm, Action<ITEmployee> action, List<string> all_positions, List<string> all_departments)
        {
            InitializeComponent();
            this.action = action;
            this.all_positions = all_positions;
            this.all_departments = all_departments;
            comboBox1.DataSource = all_positions;
            comboBox2.DataSource = all_departments;
            if (eventForm == EventForm.ShiftDepartment)
            {
                textBox1.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                comboBox1.Enabled = false;
            }
        }
        public void SetEmployee(ITEmployee employee)
        {
            textBox1.Text = employee.FullName;
            textBox4.Text = employee.Salary.ToString();
            textBox5.Text = employee.ExperienceYears.ToString();
            var inx = all_positions.IndexOf(employee.Position.ToString());
            comboBox1.SelectedIndex = inx == -1 ? 0 : inx;
            inx = all_departments.IndexOf(employee.Department.ToString());
            comboBox2.SelectedIndex = inx == -1 ? 0 : inx;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fio = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(fio) || string.IsNullOrWhiteSpace(fio))
            {
                MessageBox.Show("фио не должно быть пустым");
                return;
            }
            var pos = (Position)comboBox1.SelectedIndex;
            var depart = (Department)comboBox2.SelectedIndex;
            if (!decimal.TryParse(textBox4.Text, out decimal salary))
            {
                MessageBox.Show("Введите целое число для зарплаты");
                return;
            }
            if (!int.TryParse(textBox5.Text, out int exp))
            {
                MessageBox.Show("Введите целое число для стажа");
                return;
            }
            action(new ITEmployee { FullName = fio, Position = pos, Department = depart, Salary = salary, ExperienceYears = exp });
            this.Close();
        }
    }
    public enum EventForm
    {
        AddOrUpdate,
        ShiftDepartment
    }
}
