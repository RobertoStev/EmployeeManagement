using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.Models
{
    public class LeaveRequest 
    {
        public int LeaveRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; } 
        public LeaveType LeaveType { get; set; } 
        public LeaveStatus LeaveStatus { get; set; } 


        public int EmployeeId { get; set; } //FK
        public Employee Employee { get; set; } //Navigational property
    }
}
