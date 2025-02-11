using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly AppDbContext _context;
        public LeaveRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
        }
        public async Task<LeaveRequest?> GetLeaveRequestsByIdAsync(int id)
        {
            return await _context.LeaveRequests.FirstOrDefaultAsync(lr => lr.LeaveRequestId == id);
        }
        public async Task<List<LeaveRequest>> GetAllLeaveRequestsWithEmployeeAsync()
        {
           return await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .ToListAsync();
        }

        public async Task<LeaveRequest?> GetLeaveRequestWithEmployeeAsync(int id)
        {
            return await _context.LeaveRequests
                 .Include(lr => lr.Employee)
                 .FirstOrDefaultAsync(lr => lr.LeaveRequestId == id);
        }

        public async Task UpdateLeaveRequestAndEmployeeAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            _context.Employees.Update(leaveRequest.Employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }
    }
}
