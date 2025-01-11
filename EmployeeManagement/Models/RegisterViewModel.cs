using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Employee name is required.")]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
