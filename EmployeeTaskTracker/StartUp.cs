using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace EmployeeTaskTracker
{
    class StartUp
    {
        static void Main(string[] args)
        {
            // Create a new employee
            var employee = new Employee("John Doe", "johndoe@example.com", "555-1234", new DateTime(1990, 1, 1), 5000.0m);
            employee.Create(employee);

            // Get all employees
            var employees = employee.GetEmployees();
            foreach (var e in employees)
            {
                Console.WriteLine($"ID: {e.Id}, Name: {e.FullName}, Email: {e.Email}, Phone: {e.PhoneNumber}, DOB: {e.DateOfBirth.ToShortDateString()}, Salary: {e.MonthlySalary:C}");
            }

            // Create a new task and assign it to the first employee in the list
            var task = new Task("Complete project", "Finish project by the deadline", employees[0], new DateTime(2023, 3, 31));
            task.Create(task);

            // Get top performers last month
            employee.GetTopPerformersLastMonth();

            // Update the salary of the first employee in the list
            employees[0].MonthlySalary = 6000.0m;
            employees[0].Update();

            //Delete employees
            employee.Delete(1);
        }
    }
}
