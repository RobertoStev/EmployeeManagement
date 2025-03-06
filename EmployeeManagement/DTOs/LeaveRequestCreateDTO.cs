using System.ComponentModel.DataAnnotations;
using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.DTOs
{
    public class LeaveRequestCreateDTO
    {
        public int LeaveRequestId { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        [Display(Name = "Leave Type")]
        public LeaveType LeaveType { get; set; } 
        public LeaveStatus LeaveStatus { get; set; }

    }
}
