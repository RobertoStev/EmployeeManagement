using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DTOs
{
    public class EmployeeEditDTO
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AnnualLeaveDaysRemaining { get; set; }
        public int BonusLeaveDaysRemaining { get; set; }
    }
}
