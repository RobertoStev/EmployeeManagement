using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using static EmployeeManagement.Enums.EnumTypes;

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

        public async Task DeleteEmployeeByIdAsync(int employeeId)
        {  
            var employee = await _context.Employees
            .Include(e => e.LeaveRequests)
            .Include(e => e.SickLeaves)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee != null)
            {
                // Handle leave requests
                foreach (var request in employee.LeaveRequests.ToList())
                {
                    if (request.LeaveStatus == LeaveStatus.Approved)
                    {
                        request.Employee = null;
                    }
                    else
                    {
                        // Remove non-approved requests
                        _context.LeaveRequests.Remove(request);
                    }
                }

                foreach (var request in employee.SickLeaves.ToList())
                {
                    if (request.LeavStatus == LeaveStatus.Approved)
                    {
                        request.Employee = null;
                    }
                    else
                    {
                        // Remove non-approved requests
                        _context.SickLeaves.Remove(request);
                    }
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            
        }
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
