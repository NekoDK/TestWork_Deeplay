using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestWork_Deeplay
{
    public interface IEmployeeRepository
    {
        void AddEmployee(Employee employee);
        void UpdateEmployeePost(int empId, int postId);
        void AssignEmployeeToDepartment(int empId, int deptId, string role);
        void DeleteEmployee(int empId);
        int GetLastInsertedEmployeeId();
        void UpdateEmployee(Employee employee);
        IEnumerable<Employee> GetAllEmployees();
        void AssignEmployeeToRole(int empId, int deptId, string role, bool? inspect = null);
        DataTable GetEmployees();
        DataTable GetEmployeeDepartments();
        DataTable GetEmployeeControls();
        DataTable GetEmployeeManagers();
        string GetManagerLastName(int empId);
        bool GetInspectionStatus(int empId);
        string GetDepartmentName(int empId);
        int GetTotalEmployees();
    }

    public interface IEmployeeRoleRepository
    {
        void DeleteEmployeeRoles(int empId);
        void AddEmployeeRole(int empId, int deptId, string role, bool? inspect = null);
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SqlConnection _connection;

        public EmployeeRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public void AddEmployee(Employee employee)
        {
            var query = "INSERT INTO employees (first_name, last_name, gender, birthday, phone, post_id) " +
                        "VALUES (@FirstName, @LastName, @Gender, @Birthday, @Phone, @PostId)";

            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.Parameters.AddWithValue("@Birthday", employee.Birthday);
                command.Parameters.AddWithValue("@Phone", employee.Phone);
                command.Parameters.AddWithValue("@PostId", employee.PostId);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateEmployeePost(int empId, int postId)
        {
            var query = "UPDATE employees SET post_id = @PostId WHERE emp_id = @EmpId";
            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@PostId", postId);
                command.Parameters.AddWithValue("@EmpId", empId);
                command.ExecuteNonQuery();
            }
        }

        public void AssignEmployeeToDepartment(int empId, int deptId, string role)
        {
            var query = $"INSERT INTO {role} (emp_id, dept_id) VALUES (@EmpId, @DeptId)";
            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@EmpId", empId);
                command.Parameters.AddWithValue("@DeptId", deptId);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteEmployee(int empId)
        {
            var queries = new[]
            {
            "DELETE FROM employees WHERE emp_id = @EmpId",
            "DELETE FROM P_ctrl WHERE emp_id = @EmpId",
            "DELETE FROM P_manager WHERE emp_id = @EmpId",
            "DELETE FROM P_director WHERE emp_id = @EmpId",
            "DELETE FROM P_emp WHERE emp_id = @EmpId"
        };

            foreach (var query in queries)
            {
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@EmpId", empId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int GetLastInsertedEmployeeId()
        {
            var query = "SELECT MAX(emp_id) FROM employees";
            using (var command = new SqlCommand(query, _connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            var query = "UPDATE employees SET first_name = @FirstName, last_name = @LastName, " +
                        "gender = @Gender, birthday = @Birthday, phone = @Phone " +
                        "WHERE emp_id = @EmpId";

            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.Parameters.AddWithValue("@Birthday", employee.Birthday);
                command.Parameters.AddWithValue("@Phone", employee.Phone);
                command.Parameters.AddWithValue("@EmpId", employee.EmpId);

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();
            var query = "SELECT emp_id, first_name, last_name, gender, birthday, phone, post_id FROM employees";

            using (var command = new SqlCommand(query, _connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        EmpId = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Gender = reader.GetString(3),
                        Birthday = reader.GetDateTime(4),
                        Phone = reader.GetString(5),
                        PostId = reader.GetInt32(6)
                    });
                }
            }

            return employees;
        }

        public void AssignEmployeeToRole(int empId, int deptId, string role, bool? inspect = null)
        {
            var query = $"INSERT INTO {role} (emp_id, dept_id, inspect) VALUES (@EmpId, @DeptId, @Inspect)";
            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@EmpId", empId);
                command.Parameters.AddWithValue("@DeptId", deptId);
                if (inspect.HasValue)
                {
                    command.Parameters.AddWithValue("@Inspect", inspect.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@Inspect", DBNull.Value);
                }
                command.ExecuteNonQuery();
            }
        }

        public DataTable GetEmployees()
        {
            var query = "SELECT emp_id as 'Номер_Работника', first_name as 'Имя', last_name as 'Фамилия', " +
                        "gender as 'Пол', birthday as 'День_рождения', phone as 'Телефон', post_id as 'Номер_Должности' " +
                        "FROM employees";
            var adapter = new SqlDataAdapter(query, _connection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public DataTable GetEmployeeDepartments()
        {
            var query = "SELECT emp_id as 'Номер_Работника', dept_id as 'Номер Отдела' FROM P_emp";
            var adapter = new SqlDataAdapter(query, _connection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public DataTable GetEmployeeControls()
        {
            var query = "SELECT emp_id as 'Номер_Работника', dept_id as 'Номер Отдела' FROM P_ctrl";
            var adapter = new SqlDataAdapter(query, _connection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public DataTable GetEmployeeManagers()
        {
            var query = "SELECT emp_id as 'Номер_Работника', dept_id as 'Номер Отдела' FROM P_manager";
            var adapter = new SqlDataAdapter(query, _connection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public string GetManagerLastName(int empId)
        {
            var query = "SELECT last_name FROM employees WHERE emp_id = (SELECT emp_id FROM P_manager WHERE dept_id = (SELECT dept_id FROM P_emp WHERE emp_id = @EmpId))";
            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@EmpId", empId);
                return command.ExecuteScalar()?.ToString();
            }
        }

        public bool GetInspectionStatus(int empId)
        {
            var query = "SELECT inspect FROM P_ctrl WHERE emp_id = @EmpId";
            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@EmpId", empId);
                return Convert.ToBoolean(command.ExecuteScalar());
            }
        }

        public string GetDepartmentName(int empId)
        {
            var query = "SELECT dept_name FROM Departments WHERE dept_id = (SELECT dept_id FROM P_emp WHERE emp_id = @EmpId)";
            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@EmpId", empId);
                return command.ExecuteScalar()?.ToString();
            }
        }

        public int GetTotalEmployees()
        {
            var query = "SELECT COUNT(*) FROM employees";
            using (var command = new SqlCommand(query, _connection))
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

    }

    public class EmployeeRoleRepository : IEmployeeRoleRepository
    {
        private readonly SqlConnection _connection;

        public EmployeeRoleRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public void DeleteEmployeeRoles(int empId)
        {
            var queries = new[]
            {
            "DELETE FROM P_ctrl WHERE emp_id = @EmpId",
            "DELETE FROM P_manager WHERE emp_id = @EmpId",
            "DELETE FROM P_director WHERE emp_id = @EmpId",
            "DELETE FROM P_emp WHERE emp_id = @EmpId"
        };

            foreach (var query in queries)
            {
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@EmpId", empId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddEmployeeRole(int empId, int deptId, string role, bool? inspect = null)
        {
            string query;
            switch (role)
            {
                case "P_emp":
                    query = "INSERT INTO P_emp (emp_id, dept_id) VALUES (@EmpId, @DeptId)";
                    break;
                case "P_ctrl":
                    query = "INSERT INTO P_ctrl (emp_id, dept_id, inspect) VALUES (@EmpId, @DeptId, @Inspect)";
                    break;
                case "P_manager":
                    query = "INSERT INTO P_manager (emp_id, dept_id) VALUES (@EmpId, @DeptId)";
                    break;
                case "P_director":
                    query = "INSERT INTO P_director (emp_id) VALUES (@EmpId)";
                    break;
                default:
                    throw new ArgumentException("Invalid role");
            }

            using (var command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@EmpId", empId);
                if (role != "P_director")
                {
                    command.Parameters.AddWithValue("@DeptId", deptId);
                }
                if (role == "P_ctrl")
                {
                    command.Parameters.AddWithValue("@Inspect", inspect);
                }
                command.ExecuteNonQuery();
            }
        }
    }
}
