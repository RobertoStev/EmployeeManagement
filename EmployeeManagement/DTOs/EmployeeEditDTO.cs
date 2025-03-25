using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.DTOs
{
    public class EmployeeEditDTO
    {
        public int EmployeeId { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; }
        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Job Title is required")]
        public string JobTitle { get; set; }

    }
}
