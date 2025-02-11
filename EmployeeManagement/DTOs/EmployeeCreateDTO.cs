namespace EmployeeManagement.DTOs
{
    public class EmployeeCreateDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public IFormFile Picture { get; set; }
    }
}
