using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWork_Deeplay
{
    public partial class Delete_Stuff : Form
    {
        private readonly EmployeeService _employeeService;

        public Delete_Stuff(EmployeeService employeeService)
        {
            InitializeComponent();
            _employeeService = employeeService;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Поле ID сотрудника должно быть заполнено.");
                    return;
                }

                int empId;
                if (!int.TryParse(textBox1.Text, out empId))
                {
                    MessageBox.Show("Неверный формат ID сотрудника.");
                    return;
                }

                _employeeService.DeleteEmployee(empId);

                MessageBox.Show("Сотрудник покинул базу данных, как и вашу компанию.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
