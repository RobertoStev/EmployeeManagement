using EmployeeManagement.DTOs;
using EmployeeManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Tests.Fakes
{
    internal class FakeSickLeaveService : ISickLeaveService
    {
        public Task<bool> ApproveSickLeaveAsync(int id)
        {
            return Task.FromResult(true);
        }

        public Task CreateSickLeaveAsync(int employeeId, SickLeaveCreateDTO sickLeaveDto)
        {
            return Task.CompletedTask;
        }

        public Task<bool> DeclineSickLeaveAsync(int id)
        {
            return Task.FromResult(true);
        }
    }
}
