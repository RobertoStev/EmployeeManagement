using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.DTOs
{
    public class SickLeaveCreateDTO
    {
        public int SickLeaveId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public IFormFile MedicalReport { get; set; } // For file upload
        public LeaveStatus LeaveStatus { get; set; } 
    }
}
