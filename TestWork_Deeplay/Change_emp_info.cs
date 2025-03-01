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
    public partial class Change_emp_info : Form
    {
        private readonly EmployeeService _employeeService;

        public Change_emp_info(EmployeeService employeeService)
        {
            InitializeComponent();
            _employeeService = employeeService;
        }

        private void Change_emp_info_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            dataGridView1.DataSource = employees;
            dataGridView1.Columns["PostId"].Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox6.Text = row.Cells["EmpId"].Value.ToString();
                textBox1.Text = row.Cells["FirstName"].Value.ToString();
                textBox2.Text = row.Cells["LastName"].Value.ToString();
                textBox3.Text = row.Cells["Gender"].Value.ToString();
                textBox4.Text = row.Cells["Birthday"].Value.ToString();
                textBox5.Text = row.Cells["Phone"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) ||
                    string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text))
                {
                    MessageBox.Show("Все поля должны быть заполнены.");
                    return;
                }

                DateTime birthday;
                if (!DateTime.TryParse(textBox4.Text, out birthday))
                {
                    MessageBox.Show("Неверный формат даты.");
                    return;
                }

                int empId;
                if (!int.TryParse(textBox6.Text, out empId))
                {
                    MessageBox.Show("Неверный формат ID сотрудника.");
                    return;
                }

                var employee = new Employee
                {
                    EmpId = empId,
                    FirstName = textBox1.Text,
                    LastName = textBox2.Text,
                    Gender = textBox3.Text,
                    Birthday = birthday,
                    Phone = textBox5.Text
                };

                _employeeService.UpdateEmployee(employee);

                MessageBox.Show("Данные обновлены.");
                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
