using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTaskTracker
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Employee Assignee { get; set; }

        public DateTime DueDate { get; set; }

        public Task()
        {

        }

        public Task(string title, string description, Employee assignee, DateTime dueDate)
        {
            Title = title;
            Description = description;
            Assignee = assignee;
            DueDate = dueDate;
        }

        public void Create(Task task)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Tasks (Title, [Description], AssigneeId, DueDate) VALUES (@Title, @Description, @AssigneeId, @DueDate)", connection);
                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@AssigneeId", task.Assignee.Id);
                command.Parameters.AddWithValue("@DueDate", task.DueDate);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            string query = "UPDATE Tasks SET AssigneeId = NULL WHERE Id = @Id;" +
                           "DELETE FROM Tasks WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Task> GetTasks()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            List<Task> tasks = new List<Task>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Tasks", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Task task = new Task();
                    task.Id = (int)reader["Id"];
                    task.Title = (string)reader["Title"];
                    task.Description = (string)reader["Description"];

                    Employee assignee = new Employee();
                    assignee.Id= (int)reader["AssigneeId"];
                    assignee.FullName = (string)reader["FullName"];
                    assignee.Email = (string)reader["Email"];
                    assignee.PhoneNumber= (string)reader["PhoneNumber"];
                    assignee.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    assignee.MonthlySalary = (decimal)reader["MonthlySalary"];

                    task.Assignee = assignee;
                    task.DueDate = (DateTime)reader["DueDate"];
                    tasks.Add(task);
                }
            }
            return tasks;
        }

        public void Update()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Tasks SET Title=@Title, Description=@Description, Assignee=@Assignee, DueDate=@DueDate WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@Assignee", Assignee);
                command.Parameters.AddWithValue("@DueDate", DueDate);
                command.ExecuteNonQuery();
            }
        }
    }
}
