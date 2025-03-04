﻿using System;
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
    public partial class INSERT_Ctrl : Form
    {
        private readonly EmployeeService _employeeService;

        public INSERT_Ctrl(EmployeeService employeeService)
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
                string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) ||
                    string.IsNullOrEmpty(textBox7.Text))
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

                int deptId;
                if (!int.TryParse(textBox7.Text, out deptId))
                {
                    MessageBox.Show("Неверный формат ID отдела.");
                    return;
                }

                bool inspect;
                if (!bool.TryParse(textBox6.Text, out inspect))
                {
                    MessageBox.Show("Неверный формат инспекции.");
                    return;
                }

                _employeeService.AddController(
                    textBox1.Text, // Имя
                    textBox2.Text, // Фамилия
                    textBox3.Text, // Пол
                birthday,     // Дата рождения
                    textBox5.Text, // Телефон
                    deptId,        // ID отдела
                    inspect
                );

                MessageBox.Show("Контроллер добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
