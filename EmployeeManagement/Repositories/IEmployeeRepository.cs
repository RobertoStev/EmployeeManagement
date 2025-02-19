using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee?> GetEmployeeWithRequestsAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task DeleteEmployeeByIdAsync(int EmployeeId);
        Task UpdateEmployeeAsync(Employee employee);
    }
}
