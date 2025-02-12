using EmployeeManagement.Models;
using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.DTOs
{
    public class LeaveRequestGetDTO
    {
        public int LeaveRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public LeaveType LeaveType { get; set; }
        public LeaveStatus LeaveStatus { get; set; }


        public int EmployeeId { get; set; } 
        public Employee Employee { get; set; }
    }
}
