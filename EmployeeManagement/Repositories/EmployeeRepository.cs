using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(emp => emp.EmployeeId == id);
        }

        public async Task<Employee?> GetEmployeeWithRequestsAsync(int id)
        {
            return await _context.Employees
                .Include(emp => emp.LeaveRequests)
                .Include(emp => emp.SickLeaves)
                .FirstOrDefaultAsync(emp => emp.EmployeeId == id);
        }
        public async Task AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeByIdAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            /*var existingEmployee = await _context.Employees
           .FirstOrDefaultAsync(emp => emp.EmployeeId == employee.EmployeeId);

            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }
         
            existingEmployee.Name = employee.Name;
            existingEmployee.AnnualLeaveDaysRemaining = employee.AnnualLeaveDaysRemaining;
            existingEmployee.BonusLeaveDaysRemaining = employee.BonusLeaveDaysRemaining;
            existingEmployee.FirstPartLeaveExpiry = employee.FirstPartLeaveExpiry;
            existingEmployee.SecondPartLeaveExpiry = employee.SecondPartLeaveExpiry;*/

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
