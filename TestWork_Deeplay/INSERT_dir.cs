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

namespace TestWork_Deeplay
{
    public partial class INSERT_dir : Form
    {
        private readonly EmployeeService _employeeService;

        public INSERT_dir(EmployeeService employeeService)
        {
            InitializeComponent();
            _employeeService = employeeService;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) ||
                    string.IsNullOrEmpty(textBox5.Text))
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

                _employeeService.AddDirector(
                    textBox1.Text, // Имя
                    textBox2.Text, // Фамилия
                    textBox3.Text, // Пол
                birthday,     // Дата рождения
                    textBox5.Text // Телефон
                );

                MessageBox.Show("Директор добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
