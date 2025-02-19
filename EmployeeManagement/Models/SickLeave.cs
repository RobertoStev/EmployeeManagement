using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.Models
{
    public class SickLeave
    {
        public int SickLeaveId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string MedicalReportPath { get; set; }
        public LeaveStatus LeavStatus { get; set; } 
        public Employee? Employee { get; set; } 
    }
}
