using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DTOs
{
    public class EmployeeEditDTO
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }

    }
}
