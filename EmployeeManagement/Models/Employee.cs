using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AnnualLeaveDaysRemaining { get; set; }
        public int BonusLeaveDaysRemaining { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FirstPartLeaveExpiry { get; set; }
        public DateTime? SecondPartLeaveExpiry { get; set; }
        public string Picture { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public ICollection<SickLeave> SickLeaves { get; set; }

        
    }
}
