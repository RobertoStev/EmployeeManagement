using EmployeeManagement.DTOs;

namespace EmployeeManagement.Services
{
    public interface ISickLeaveService
    {
        Task CreateSickLeaveAsync(int employeeId, SickLeaveCreateDTO sickLeaveDto);
        Task<bool> ApproveSickLeaveAsync(int id);
        Task<bool> DeclineSickLeaveAsync(int id);
    }
}
