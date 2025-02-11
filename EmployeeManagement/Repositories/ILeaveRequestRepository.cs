using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories
{
    public interface ILeaveRequestRepository
    {
        Task<List<LeaveRequest>> GetAllLeaveRequestsWithEmployeeAsync();
        Task<LeaveRequest?> GetLeaveRequestWithEmployeeAsync(int id);
        Task<LeaveRequest?> GetLeaveRequestsByIdAsync(int id);
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task UpdateLeaveRequestAndEmployeeAsync(LeaveRequest leaveRequest);
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);


    }
}
