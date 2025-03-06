using System.ComponentModel.DataAnnotations;
using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.DTOs
{
    public class SickLeaveCreateDTO
    {
        public int SickLeaveId { get; set; }
        [Display(Name = "Start Date")]      
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]     
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        [Display(Name = "Medical Report")]
        public IFormFile MedicalReport { get; set; } 
        public LeaveStatus LeaveStatus { get; set; } 
    }
}
