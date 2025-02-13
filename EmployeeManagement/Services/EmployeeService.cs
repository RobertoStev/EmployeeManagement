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

        public async Task CreateEmployeeAsync(Employee domainEmployee)
        {
            domainEmployee.AnnualLeaveDaysRemaining = 21;
            domainEmployee.BonusLeaveDaysRemaining = 0;
            domainEmployee.CreatedAt = DateTime.Now;

            domainEmployee.FirstPartLeaveExpiry = domainEmployee.CreatedAt.AddMinutes(3);
            domainEmployee.SecondPartLeaveExpiry = domainEmployee.CreatedAt.AddMinutes(5);

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

            if(employee.FirstPartLeaveExpiry != null && employee.SecondPartLeaveExpiry != null)
            {
                //Add
                if(employeeDto.BonusLeaveDaysRemaining >= 0)
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                else //Deduct
                {
                    var deductDays = employee.BonusLeaveDaysRemaining + employeeDto.BonusLeaveDaysRemaining;
                    if(deductDays < 0)
                    {
                        throw new Exception($"This employee has {employee.BonusLeaveDaysRemaining} bonus leave days you can't deduct him {employeeDto.BonusLeaveDaysRemaining}!");
                    }
                    else
                    {
                        employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                    }
                }

                //Add
                if (employeeDto.AnnualLeaveDaysRemaining >= 0)
                    employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                else //Deduct
                {
                    var deductDays = employee.AnnualLeaveDaysRemaining + employeeDto.AnnualLeaveDaysRemaining;
                    if (deductDays < 0)
                    {
                        throw new Exception($"This employee has {employee.AnnualLeaveDaysRemaining} annual leave days you can't deduct him {employeeDto.AnnualLeaveDaysRemaining}!");
                    }
                    else
                    {
                        employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                    }
                }
            }

            if (employee.FirstPartLeaveExpiry == null && employee.SecondPartLeaveExpiry != null)
            {
                //Add
                if (employeeDto.BonusLeaveDaysRemaining >= 0)
                {
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                    employee.FirstPartLeaveExpiry = employee.SecondPartLeaveExpiry.Value.AddMinutes(3);
                }
                else //No bonus leave days to deduct
                {
                    throw new Exception($"This employee has {employee.BonusLeaveDaysRemaining} bonus leave days remaining!");
                }

                //Add
                if (employeeDto.AnnualLeaveDaysRemaining >= 0)
                {
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                }
                else //Deduct
                {
                    var deductDays = employee.AnnualLeaveDaysRemaining + employeeDto.AnnualLeaveDaysRemaining;
                    if (deductDays < 0)
                    {
                        throw new Exception($"This employee has {employee.AnnualLeaveDaysRemaining} annual leave days you can't deduct him {employeeDto.AnnualLeaveDaysRemaining}!");
                    }
                    else
                    {
                        employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                    }
                }
            }

            if (employee.FirstPartLeaveExpiry != null && employee.SecondPartLeaveExpiry == null)
            {
                //Add
                if (employeeDto.BonusLeaveDaysRemaining >= 0)
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                else //Deduct
                {
                    var deductDays = employee.BonusLeaveDaysRemaining + employeeDto.BonusLeaveDaysRemaining;
                    if (deductDays < 0)
                    {
                        throw new Exception($"This employee has {employee.BonusLeaveDaysRemaining} bonus leave days you can't deduct him {employeeDto.BonusLeaveDaysRemaining}!");
                    }
                    else
                    {
                        employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                    }
                }

                //Add
                if (employeeDto.AnnualLeaveDaysRemaining >= 0)
                {
                    employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                    employee.SecondPartLeaveExpiry = employee.FirstPartLeaveExpiry.Value.AddMinutes(2);
                }
                else //No annual leave days to deduct
                {
                    throw new Exception($"This employee has {employee.BonusLeaveDaysRemaining} annual leave days remaining!");
                }
            }

            if (employee.FirstPartLeaveExpiry == null && employee.SecondPartLeaveExpiry == null)
            {
                //No annual and bonus days to deduct
                if (employeeDto.AnnualLeaveDaysRemaining <= 0 && employeeDto.BonusLeaveDaysRemaining <= 0)
                {
                    throw new Exception($"This employee has 0 annual and bonus leave days remaining!");
                }

                //Add
                if (employeeDto.AnnualLeaveDaysRemaining > 0 || employeeDto.BonusLeaveDaysRemaining > 0)
                {
                    employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;

                    employee.FirstPartLeaveExpiry = now.AddMinutes(3);
                    employee.SecondPartLeaveExpiry = now.AddMinutes(5);
                }
            }

            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
    }
}
