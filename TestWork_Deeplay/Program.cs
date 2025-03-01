using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace TestWork_Deeplay
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);


            var serviceProvider = services.BuildServiceProvider();


            var mainForm = serviceProvider.GetRequiredService<Form1>();
            Application.Run(mainForm);
        }


        private static void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<SqlConnection>(provider =>
                new SqlConnection(ConfigurationManager.ConnectionStrings["DBdp"].ConnectionString));

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
            services.AddScoped<EmployeeRoleService>();
            services.AddScoped<EmployeeService>();

            services.AddTransient<Form1>();
            services.AddTransient<INSERT_EMP>();
            services.AddTransient<INSERT_Manager>();
            services.AddTransient<INSERT_dir>();
            services.AddTransient<INSERT_Ctrl>();
            services.AddTransient<Delete_Stuff>();
            services.AddTransient<Change_emp_info>();
            services.AddTransient<Post_Change>();

        }
    }
}