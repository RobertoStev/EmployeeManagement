using EmployeeManagement.Models;
using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.DTOs
{
    public class SickLeaveGetDTO
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
