using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWork_Deeplay
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public void AddManager(string firstName, string lastName, string gender, DateTime birthday, string phone, int deptId)
        {
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Birthday = birthday,
                Phone = phone,
                PostId = 3 // ID должности менеджера
            };

            _employeeRepository.AddEmployee(employee);

            var empId = _employeeRepository.GetLastInsertedEmployeeId();
            _employeeRepository.AssignEmployeeToDepartment(empId, deptId, "P_manager");
        }

        public void AddEmployee(string firstName, string lastName, string gender, DateTime birthday, string phone, int deptId)
        {
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Birthday = birthday,
                Phone = phone,
                PostId = 1 // ID должности сотрудника
            };

            _employeeRepository.AddEmployee(employee);

            var empId = _employeeRepository.GetLastInsertedEmployeeId();
            _employeeRepository.AssignEmployeeToDepartment(empId, deptId, "P_emp");
        }

        public void AddDirector(string firstName, string lastName, string gender, DateTime birthday, string phone)
        {
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Birthday = birthday,
                Phone = phone,
                PostId = 4 // ID должности директора
            };

            _employeeRepository.AddEmployee(employee);

            var empId = _employeeRepository.GetLastInsertedEmployeeId();

            _employeeRepository.AssignEmployeeToRole(empId, 0, "P_director");
        }

        public void DeleteEmployee(int empId)
        {
            _employeeRepository.DeleteEmployee(empId);
        }

        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.UpdateEmployee(employee);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }
        public void AddController(string firstName, string lastName, string gender, DateTime birthday, string phone, int deptId, bool inspect)
        {
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Birthday = birthday,
                Phone = phone,
                PostId = 2 // ID должности контроллера
            };

            _employeeRepository.AddEmployee(employee);

            var empId = _employeeRepository.GetLastInsertedEmployeeId();
            _employeeRepository.AssignEmployeeToRole(empId, deptId, "P_ctrl", inspect);
        }

        public DataTable GetEmployees()
        {
            return _employeeRepository.GetEmployees();
        }

        public DataTable GetEmployeeDepartments()
        {
            return _employeeRepository.GetEmployeeDepartments();
        }

        public DataTable GetEmployeeControls()
        {
            return _employeeRepository.GetEmployeeControls();
        }

        public DataTable GetEmployeeManagers()
        {
            return _employeeRepository.GetEmployeeManagers();
        }

        public string GetManagerLastName(int empId)
        {
            return _employeeRepository.GetManagerLastName(empId);
        }

        public bool GetInspectionStatus(int empId)
        {
            return _employeeRepository.GetInspectionStatus(empId);
        }

        public string GetDepartmentName(int empId)
        {
            return _employeeRepository.GetDepartmentName(empId);
        }

        public int GetTotalEmployees()
        {
            return _employeeRepository.GetTotalEmployees();
        }
    }
}
