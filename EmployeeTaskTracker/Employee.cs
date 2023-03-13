using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaskTracker
{
    public class Employee
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal MonthlySalary { get; set; }

        public Employee()
        {
        }

        public Employee(string fullName, string email, string phoneNumber, DateTime dateOfBirth, decimal monthlySalary)
        {
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            MonthlySalary = monthlySalary;
        }

        public void Create(Employee employee)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Employees (FullName, Email, PhoneNumber, DateOfBirth, MonthlySalary) VALUES (@FullName, @Email, @PhoneNumber, @DateOfBirth, @MonthlySalary)", connection);
                command.Parameters.AddWithValue("@FullName", employee.FullName);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                command.Parameters.AddWithValue("@MonthlySalary", employee.MonthlySalary);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            string query = "DELETE FROM Tasks WHERE AssigneeId = @Id;" +
                           "DELETE FROM Employees WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void GetTopPerformersLastMonth()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = @"SELECT TOP 5 e.FullName, COUNT(t.Id) AS tasks_completed
                      FROM Employees e
                      JOIN Tasks t ON e.Id = t.AssigneeId
                      WHERE t.DueDate >= DATEADD(month, -1, GETDATE())
                      GROUP BY e.Id, e.FullName
                      ORDER BY tasks_completed DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Top performers in the past month:");
                        while (reader.Read())
                        {
                            var employeeName = reader.GetString(0);
                            var tasksCompleted = reader.GetInt32(1);
                            Console.WriteLine($"{employeeName} completed {tasksCompleted} tasks");
                        }
                    }
                }
            }
        }



        public List<Employee> GetEmployees()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            List<Employee> employees = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Employees", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.Id = (int)reader["Id"];
                    employee.FullName = (string)reader["FullName"];
                    employee.Email = (string)reader["Email"];
                    employee.PhoneNumber = (string)reader["PhoneNumber"];
                    employee.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    employee.MonthlySalary = (decimal)reader["MonthlySalary"];
                    employees.Add(employee);
                }
            }
            return employees;
        }

        public void Update()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Employees SET FullName=@FullName, Email=@Email, PhoneNumber=@PhoneNumber, DateOfBirth=@DateOfBirth, MonthlySalary=@MonthlySalary WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@FullName", FullName);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                command.Parameters.AddWithValue("@MonthlySalary", MonthlySalary);
                command.ExecuteNonQuery();
            }
        }
    }
}
