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
    public class FakeSickLeaveRepository : ISickLeaveRequestRepository
    {
        private readonly List<SickLeave> _sickLeaves;
        public FakeSickLeaveRepository()
        {
            _sickLeaves = new List<SickLeave>
            {
                new SickLeave
                {
                    SickLeaveId = 1,
                    StartDate = new DateTime(2025, 1, 10),
                    EndDate = new DateTime(2025, 1, 15),
                    Reason = "Flu",
                    MedicalReportPath = "path/to/flu_report.pdf",
                    LeavStatus = LeaveStatus.Approved,
                    //EmployeeId = 1,
                    Employee = new Employee
                    {
                        EmployeeId = 1,
                        Name = "John Doe",
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
                new SickLeave
                {
                    SickLeaveId = 2,
                    StartDate = new DateTime(2025, 2, 5),
                    EndDate = new DateTime(2025, 2, 7),
                    Reason = "Back Pain",
                    MedicalReportPath = "path/to/back_pain_report.pdf",
                    LeavStatus = LeaveStatus.Pending,
                    //EmployeeId = 2,
                    Employee = new Employee
                    {
                        EmployeeId = 2,
                        Name = "Jane Smith",
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
        public Task AddSickLeaveAsync(SickLeave sickLeave)
        {
            _sickLeaves.Add(sickLeave);
            return Task.CompletedTask;
        }

        public Task<List<SickLeave>> GetAllSickLeavesAndEmployeeAsync()
        {
            return Task.FromResult(_sickLeaves);
        }

        public Task<SickLeave?> GetSickLeaveByIdAsync(int id)
        {
            var sickLeave = _sickLeaves.FirstOrDefault(sl => sl.SickLeaveId == id);
            return Task.FromResult(sickLeave);
        }

        public Task UpdateSickLeaveAsync(SickLeave sickLeave)
        {
            var existingSickLeave = _sickLeaves.FirstOrDefault(sl => sl.SickLeaveId == sickLeave.SickLeaveId);

            _sickLeaves.Remove(existingSickLeave);
            _sickLeaves.Add(sickLeave);
            return Task.CompletedTask;
        }
    }
}
