using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace TestWork_Deeplay
{
    public partial class Form1 : Form
    {
        private readonly EmployeeService _employeeService;

        public Form1(EmployeeService employeeService)
        {
            InitializeComponent();
            _employeeService = employeeService;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dataGridView6.DataSource = _employeeService.GetEmployees();
            dataGridView9.DataSource = _employeeService.GetEmployeeDepartments();
            dataGridView8.DataSource = _employeeService.GetEmployeeControls();
            dataGridView7.DataSource = _employeeService.GetEmployeeManagers();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            (dataGridView6.DataSource as DataTable).DefaultView.RowFilter = $"Фамилия LIKE '%{textBox1.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filter = comboBox1.SelectedIndex switch
            {
                0 => "Номер_Должности = 1",
                1 => "Номер_Должности = 2",
                2 => "Номер_Должности = 3",
                3 => "Номер_Должности = 4",
                _ => ""
            };

            (dataGridView6.DataSource as DataTable).DefaultView.RowFilter = filter;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            (dataGridView6.DataSource as DataTable).DefaultView.RowFilter = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filter = comboBox2.SelectedIndex switch
            {
                0 => "Номер_Должности = 1",
                1 => "Номер_Должности = 2",
                2 => "Номер_Должности = 3",
                3 => "Номер_Должности = 4",
                _ => ""
            };

            (dataGridView6.DataSource as DataTable).DefaultView.RowFilter = filter;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int empId))
            {
                textBox3.Text = _employeeService.GetManagerLastName(empId);
            }
            else
            {
                MessageBox.Show("Неверный формат ID сотрудника.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox5.Text, out int empId))
            {
                textBox4.Text = _employeeService.GetInspectionStatus(empId).ToString();
            }
            else
            {
                MessageBox.Show("Неверный формат ID сотрудника.");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox7.Text, out int empId))
            {
                textBox6.Text = _employeeService.GetDepartmentName(empId);
            }
            else
            {
                MessageBox.Show("Неверный формат ID сотрудника.");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox8.Text = _employeeService.GetTotalEmployees().ToString();
        }
    }
}
