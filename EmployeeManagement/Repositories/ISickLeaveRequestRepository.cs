using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories
{
    public interface ISickLeaveRequestRepository
    {
        Task<List<SickLeave>> GetAllSickLeavesAndEmployeeAsync();
        Task<SickLeave?> GetSickLeaveByIdAsync(int id);
        Task AddSickLeaveAsync(SickLeave sickLeave);
        Task UpdateSickLeaveAsync(SickLeave sickLeave);
    }
}
