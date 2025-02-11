using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.Tests.Fakes
{
    internal class FakeLeaveRequestService : ILeaveRequestService
    {
        public Task<bool> ApproveLeaveRequestAsync(int id)
        { 
            return Task.FromResult(true); 
        }

        public Task<LeaveRequest> CreateLeaveRequestAsync(int employeeId, LeaveRequestCreateDTO leaveRequest)
        {  
            return Task.FromResult(new LeaveRequest
            {
                LeaveRequestId = 1,
                StartDate = new DateTime(2025, 1, 10),
                EndDate = new DateTime(2025, 1, 15),
                Comment = "Family vacation",
                LeaveType = LeaveType.Annual,
                LeaveStatus = LeaveStatus.Pending,
                EmployeeId = 1
            });
        }

        public Task<bool> DeclineLeaveRequestAsync(int id)
        {
            return Task.FromResult(true);
        }
    }
}
