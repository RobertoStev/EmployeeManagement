using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository repository)
        {
            _employeeRepository = repository;
        }

        //Vo repository?
        public async Task CreateEmployeeAsync(Employee domainEmployee)
        {
            domainEmployee.AnnualLeaveDaysRemaining = 21;
            domainEmployee.BonusLeaveDaysRemaining = 0;
            domainEmployee.CreatedAt = DateTime.Now;

            domainEmployee.FirstPartLeaveExpiry = domainEmployee.CreatedAt.AddHours(1);
            domainEmployee.SecondPartLeaveExpiry = domainEmployee.CreatedAt.AddHours(2); 

            await _employeeRepository.AddEmployeeAsync(domainEmployee);
        }

        public async Task MangeDaysForEmployeeAsync(EmployeeManageDaysDTO employeeDto)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeDto.EmployeeId);
            if(employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            var now = DateTime.Now;
            if (employee.FirstPartLeaveExpiry == null && employeeDto.AnnualLeaveDaysRemaining > 0)
            {
                employee.FirstPartLeaveExpiry = now.AddHours(1);
            }
            if (employee.FirstPartLeaveExpiry == null && employeeDto.BonusLeaveDaysRemaining > 0)
            {
                employee.FirstPartLeaveExpiry = now.AddHours(2);
            }

            employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
            employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;

            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
    }
}
