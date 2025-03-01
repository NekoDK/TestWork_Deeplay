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
    public partial class Post_Change : Form
    {
        private readonly EmployeeRoleService _employeeRoleService;

        public Post_Change(EmployeeRoleService employeeRoleService)
        {
            InitializeComponent();
            _employeeRoleService = employeeRoleService;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int empId = int.Parse(textBox1.Text);
            int deptId = int.Parse(textBox2.Text);

            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0: // Работяга
                        _employeeRoleService.ChangeEmployeeRole(empId, deptId, "P_emp");
                        MessageBox.Show("Сотрудник стал работягой");
                        break;

                    case 1: // Контролёр
                        bool inspect = bool.Parse(textBox3.Text);
                        _employeeRoleService.ChangeEmployeeRole(empId, deptId, "P_ctrl", inspect);
                        MessageBox.Show("Сотрудник стал контролёром");
                        break;

                    case 2: // Менеджер
                        _employeeRoleService.ChangeEmployeeRole(empId, deptId, "P_manager");
                        MessageBox.Show("Сотрудник стал руководителем отдела");
                        break;

                    case 3: // Директор
                        _employeeRoleService.ChangeEmployeeRole(empId, deptId, "P_director");
                        MessageBox.Show("Сотрудник стал директором");
                        break;

                    default:
                        MessageBox.Show("Неверный выбор роли");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
