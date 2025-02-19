using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Tests.Fakes
{
    internal class FakeEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employees;
        public FakeEmployeeRepository()
        {
            _employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    AnnualLeaveDaysRemaining = 21,  
                    BonusLeaveDaysRemaining = 0,    
                    CreatedAt = DateTime.Now,       
                    FirstPartLeaveExpiry = DateTime.Now.AddHours(1),
                    SecondPartLeaveExpiry = DateTime.Now.AddHours(2),
                    LeaveRequests = new List<LeaveRequest>(),
                    SickLeaves = new List<SickLeave>()
                },
                new Employee
                {
                    EmployeeId = 2,
                    Name = "Jane Smith",
                    Email = "jane@example.com",
                    AnnualLeaveDaysRemaining = 21,  
                    BonusLeaveDaysRemaining = 0,    
                    CreatedAt = DateTime.Now, 
                    FirstPartLeaveExpiry = DateTime.Now.AddHours(1),
                    SecondPartLeaveExpiry = DateTime.Now.AddHours(2),
                    LeaveRequests = new List<LeaveRequest>(),
                    SickLeaves = new List<SickLeave>()
                }
            };
        }
        public Task AddEmployeeAsync(Employee employee)
        {
            _employees.Add(employee);
            return Task.CompletedTask;
        }

        public Task DeleteEmployeeByIdAsync(int employeeId)
        {
            var employee = _employees[employeeId];
            _employees.Remove(employee);
            return Task.CompletedTask;
        }

        public Task<List<Employee>> GetAllEmployeesAsync()
        {
            return Task.FromResult(_employees);
        }

        public Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            var employee = _employees.FirstOrDefault(e => e.Email == email);
            return Task.FromResult(employee);
        }

        public Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.EmployeeId == id);
            return Task.FromResult(employee);
        }

        public Task<Employee?> GetEmployeeWithRequestsAsync(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.EmployeeId == id);
            return Task.FromResult(employee);
        }

        public Task UpdateEmployeeAsync(Employee employee)
        {
            var excisting = _employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

            _employees.Remove(excisting);
            _employees.Add(employee);
            return Task.CompletedTask;
        }

        public void ClearEmployees()
        {
            _employees.Clear();
        }
    }
}
