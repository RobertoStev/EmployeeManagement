using EmployeeManagement.DTOs;
using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequest> CreateLeaveRequestAsync(int employeeId, LeaveRequestCreateDTO leaveRequest);
        Task<bool> ApproveLeaveRequestAsync(int id);
        Task<bool> DeclineLeaveRequestAsync(int id);
    }
}
