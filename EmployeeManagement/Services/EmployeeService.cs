using AutoMapper;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _employeeRepository = repository;
            _mapper = mapper;
        }

        public async Task CreateEmployeeAsync(EmployeeCreateDTO employeeDto, string imagesFolderPath)
        {           
            var domainEmployee = _mapper.Map<Employee>(employeeDto);

            // Handle file upload
            if (employeeDto.Picture != null && employeeDto.Picture.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(employeeDto.Picture.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    throw new Exception($"Only image files(JPG, PNG, GIF) are allowed");
                }

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(imagesFolderPath, fileName);

                // Save the file to the wwwroot/images folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employeeDto.Picture.CopyToAsync(stream);
                }

                domainEmployee.Picture = fileName;
            }
            else
            {
                domainEmployee.Picture = "default.jpg";
            }

            // Set additional properties
            domainEmployee.AnnualLeaveDaysRemaining = 21;
            domainEmployee.BonusLeaveDaysRemaining = 0;
            domainEmployee.CreatedAt = DateTime.Today;
            domainEmployee.FirstPartLeaveExpiry = new DateTime(domainEmployee.CreatedAt.Year, 12, 31);
            domainEmployee.SecondPartLeaveExpiry = new DateTime(domainEmployee.CreatedAt.Year + 1, 6, 30);

            await _employeeRepository.AddEmployeeAsync(domainEmployee);
        }

        public async Task MangeDaysForEmployeeAsync(EmployeeManageDaysDTO employeeDto)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeDto.EmployeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            // var now = DateTime.Now;
            var now = DateTime.Today;

            if (employee.FirstPartLeaveExpiry != null && employee.SecondPartLeaveExpiry != null)
            {
                // Add
                if (employeeDto.BonusLeaveDaysRemaining >= 0)
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                else // Deduct
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

                // Add
                if (employeeDto.AnnualLeaveDaysRemaining >= 0)
                    employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                else // Deduct
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
                // Add
                if (employeeDto.BonusLeaveDaysRemaining > 0)
                {
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                    employee.FirstPartLeaveExpiry = new DateTime(now.Year, 12, 31);
                }
                if (employeeDto.BonusLeaveDaysRemaining < 0) // No bonus leave days to deduct
                {
                    throw new Exception($"This employee has {employee.BonusLeaveDaysRemaining} bonus leave days remaining!");
                }

                // Add
                if (employeeDto.AnnualLeaveDaysRemaining >= 0)
                {
                    employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                }
                else // Deduct
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
                // Add
                if (employeeDto.BonusLeaveDaysRemaining >= 0)
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;
                else // Deduct
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

                // Add
                if (employeeDto.AnnualLeaveDaysRemaining > 0)
                {
                    employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                    employee.SecondPartLeaveExpiry = employee.FirstPartLeaveExpiry.Value.AddMonths(6);
                }
                if (employeeDto.AnnualLeaveDaysRemaining < 0) // No annual leave days to deduct
                {
                    throw new Exception($"This employee has {employee.AnnualLeaveDaysRemaining} annual leave days remaining!");
                }
            }

            if (employee.FirstPartLeaveExpiry == null && employee.SecondPartLeaveExpiry == null)
            {
                // No annual and bonus days to deduct
                if (employeeDto.AnnualLeaveDaysRemaining < 0 && employeeDto.BonusLeaveDaysRemaining < 0)
                {
                    throw new Exception($"This employee has 0 annual and bonus leave days remaining!");
                }
                if (employeeDto.AnnualLeaveDaysRemaining < 0)
                {
                    throw new Exception($"This employee has 0 annual leave days remaining!");
                }
                if (employeeDto.BonusLeaveDaysRemaining < 0)
                {
                    throw new Exception($"This employee has 0 bonus leave days remaining!");
                }

                // Add
                if (employeeDto.AnnualLeaveDaysRemaining > 0 || employeeDto.BonusLeaveDaysRemaining > 0)
                {
                    employee.AnnualLeaveDaysRemaining += employeeDto.AnnualLeaveDaysRemaining;
                    employee.BonusLeaveDaysRemaining += employeeDto.BonusLeaveDaysRemaining;

                    employee.FirstPartLeaveExpiry = new DateTime(now.Year, 12, 31);
                    employee.SecondPartLeaveExpiry = new DateTime(now.Year + 1, 6, 30);
                }
            }

            await _employeeRepository.UpdateEmployeeAsync(employee);
        }

        public async Task EditEmployeeAsync(EmployeeEditDTO employeeDto)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeDto.EmployeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            _mapper.Map<Employee>(employeeDto);

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Department = employeeDto.Department;
            employee.JobTitle = employeeDto.JobTitle;

            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
    }
}
