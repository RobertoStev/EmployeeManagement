using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add any custom properties you need
        public string? UserName { get; set; } // Example custom property
        public string? Email { get; set; } // Example custom property
        public int? EmployeeId { get; set; } // Example custom property
    }
}
