using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories
{
    public class SickLeaveRequestRepository : ISickLeaveRequestRepository
    {
        private readonly AppDbContext _context;
        public SickLeaveRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddSickLeaveAsync(SickLeave sickLeave)
        {
            _context.SickLeaves.Add(sickLeave);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SickLeave>> GetAllSickLeavesAndEmployeeAsync()
        {
            return await _context.SickLeaves
                .Include(sl => sl.Employee)
                .ToListAsync();
        }

        public async Task<SickLeave?> GetSickLeaveByIdAsync(int id)
        {
            return await _context.SickLeaves
                .Include(sl => sl.Employee)
                .FirstOrDefaultAsync(sl => sl.SickLeaveId == id);
        }

        public async Task UpdateSickLeaveAsync(SickLeave sickLeave)
        {
            _context.SickLeaves.Update(sickLeave);
            await _context.SaveChangesAsync();
        }
    }
}
