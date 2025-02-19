using EmployeeManagement.DTOs;
using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public interface IEmployeeService
    {
        Task MangeDaysForEmployeeAsync(EmployeeManageDaysDTO employeeDto);
        Task CreateEmployeeAsync(EmployeeCreateDTO employee, string imagesFolderPath);
    }
}
