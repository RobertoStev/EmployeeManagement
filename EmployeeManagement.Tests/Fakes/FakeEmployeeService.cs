using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Tests.Fakes
{
    internal class FakeEmployeeService : IEmployeeService
    {
        public Task CreateEmployeeAsync(Employee employee)
        {
            // Simulate creating an employee
            return Task.CompletedTask;
        }

        public Task MangeDaysForEmployeeAsync(EmployeeManageDaysDTO employeeDto)
        {  
            return Task.CompletedTask;
        }
    }
}
