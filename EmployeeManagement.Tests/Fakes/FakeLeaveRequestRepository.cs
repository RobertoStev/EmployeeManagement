using EmployeeManagement.Enums;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.Tests.Fakes
{
    internal class FakeLeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly List<LeaveRequest> _leaveRequests;
        public FakeLeaveRequestRepository()
        {
            _leaveRequests = new List<LeaveRequest>
            {
                new LeaveRequest
                {
                    LeaveRequestId = 1,
                    StartDate = new DateTime(2025, 1, 10),
                    EndDate = new DateTime(2025, 1, 15),
                    Comment = "Family vacation",
                    LeaveType = LeaveType.Annual,
                    LeaveStatus = LeaveStatus.Pending,
                    //EmployeeId = 1,
                    Employee = new Employee
                    {
                        EmployeeId = 1,
                        FirstName = "John",
                        LastName= "Doe",
                        Department = "IT",
                        JobTitle = "Developer",
                        Email = "john@example.com",
                        AnnualLeaveDaysRemaining = 21,  
                        BonusLeaveDaysRemaining = 0,    
                        CreatedAt = DateTime.Now,       
                        FirstPartLeaveExpiry = DateTime.Now.AddHours(1),
                        SecondPartLeaveExpiry = DateTime.Now.AddHours(2),
                        LeaveRequests = new List<LeaveRequest>(),
                        SickLeaves = new List<SickLeave>()
                    }
                },
                new LeaveRequest
                {
                    LeaveRequestId = 2,
                    StartDate = new DateTime(2025, 9, 20),
                    EndDate = new DateTime(2025, 9, 28),
                    Comment = "Dd",
                    LeaveType = LeaveType.Bonus,
                    LeaveStatus = LeaveStatus.Approved,
                    //EmployeeId = 1,
                    Employee = new Employee
                    {
                        EmployeeId = 2,
                        FirstName = "Jane",
                        LastName= "Smith",
                        Department = "IT",
                        JobTitle = "Tester",
                        Email = "jane@example.com",
                        AnnualLeaveDaysRemaining = 21,  
                        BonusLeaveDaysRemaining = 0,    
                        CreatedAt = DateTime.Now, 
                        FirstPartLeaveExpiry = DateTime.Now.AddHours(1),
                        SecondPartLeaveExpiry = DateTime.Now.AddHours(2),
                        LeaveRequests = new List<LeaveRequest>(),
                        SickLeaves = new List<SickLeave>()
                    }
                }
            };
        }

        public Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _leaveRequests.Add(leaveRequest);
            return Task.CompletedTask;
        }

        public Task<List<LeaveRequest>> GetAllLeaveRequestsWithEmployeeAsync()
        {
            return Task.FromResult(_leaveRequests);
        }

        public Task<LeaveRequest?> GetLeaveRequestsByIdAsync(int id)
        {
            var leaveRequest = _leaveRequests.FirstOrDefault(lr => lr.LeaveRequestId == id);
            return Task.FromResult(leaveRequest);
        }

        public Task<LeaveRequest?> GetLeaveRequestWithEmployeeAsync(int id)
        {
            var leaveRequest = _leaveRequests.FirstOrDefault(lr => lr.LeaveRequestId == id);
            return Task.FromResult(leaveRequest); 
        }

        public Task UpdateLeaveRequestAndEmployeeAsync(LeaveRequest leaveRequest)
        {
            var existingRequest = _leaveRequests.FirstOrDefault(lr => lr.LeaveRequestId == leaveRequest.LeaveRequestId);

            _leaveRequests.Remove(existingRequest);
            _leaveRequests.Add(leaveRequest);
            return Task.CompletedTask;
        }

        public Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            var excisting = _leaveRequests.FirstOrDefault(lr => lr.LeaveRequestId == leaveRequest.LeaveRequestId);

            _leaveRequests.Remove(excisting);
            _leaveRequests.Add(leaveRequest);
            return Task.CompletedTask;
        }
    }
}
