using DataAccessLayer;
using LogicLib;
using LogicLibrary;
using Microsoft.VisualBasic.Logging;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Laba1
{
    public partial class Form1 : Form
    {
        private static List<string> all_positions = Enum.GetNames(typeof(Position)).ToList();
        private static List<string> all_departments = Enum.GetNames(typeof(Department)).ToList();
        private Logic logic;
        public Form1()
        {
            InitializeComponent();
            //logic = new Logic(new DapperRepository<ITEmployee>(), new DapperRepository<Language>());
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModuleDapper());
            logic = ninjectKernel.Get<Logic>();
            comboBox1.DataSource = all_positions;
            comboBox2.DataSource = all_departments;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            checkBox1.Checked = true;

            ShowData();
        }
        public void ShowData()
        {
            var list = new List<ITEmployee>();
            if (checkBox1.Checked)
                list = logic.GetAllEmployees();
            else if (checkBox2.Checked)
                list = logic.GetEmployeeByPosition((Position)comboBox1.SelectedIndex);
            else if (checkBox3.Checked)
                list = logic.GetEmployeeByDepartment((Department)comboBox2.SelectedIndex);
            else if (checkBox4.Checked)
                list = logic.GetPromoteEmployees();

            var lObj = new List<object>();

            foreach (var i in list)
            {
                lObj.Add(new { ID = i.Id, FullName = i.FullName, Position = i.Position, Department = i.Department, Salary = i.Salary, ExperienceYears = i.ExperienceYears, Language = i.Language.Name });
            }

            dataGridView1.DataSource = lObj;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                return;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            ShowData();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
                return;
            checkBox1.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            ShowData();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox3.Checked)
                return;
            checkBox2.Checked = false;
            checkBox1.Checked = false;
            checkBox4.Checked = false;
            ShowData();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
                return;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox1.Checked = false;
            ShowData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                ShowData();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                ShowData();
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            var form = new UpdateEmployee(EventForm.AddOrUpdate, logic.AddEmployee, all_positions, all_departments);
            form.ShowDialog();
            ShowData();
        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите работника");
                return;
            }
            Action<ITEmployee,string> update = (ITEmployee e,string s) => {
                logic.UpdateEmployee(e,s);
            };
            var form = new UpdateEmployee(EventForm.AddOrUpdate, update, all_positions, all_departments);
            form.SetEmployee(logic.GetEmployeeById((int)dataGridView1.SelectedRows[0].Cells[0].Value));
            form.ShowDialog();
            ShowData();
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить запись?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.No)
                return;

            foreach (var i in dataGridView1.SelectedRows)
            {
                DataGridViewRow row = (DataGridViewRow)i;
                int id = (int)row.Cells[0].Value;
                logic.DeleteEmployee(id);
            }
            ShowData();
        }

        private void shift_btn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null || dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите работника");
                return;
            }
            Action<ITEmployee,string> update = (ITEmployee e, string s) => {
                logic.UpdateEmployee(e);
            };
            var form = new UpdateEmployee(EventForm.ShiftDepartment, update, all_positions, all_departments);
            form.SetEmployee(logic.GetEmployeeById((int)dataGridView1.SelectedRows[0].Cells[0].Value));
            form.ShowDialog();
            ShowData();
        }

        private void up_btn_Click(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
                return;
            foreach (var i in dataGridView1.SelectedRows)
            {
                DataGridViewRow row = (DataGridViewRow)i;
                int id = (int)row.Cells[0].Value;
                logic.PromoteEmployeeBasedOnExperience(id);
            }
            ShowData();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
