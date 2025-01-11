using AutoMapper;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;

namespace EmployeeManagement.AutoMapper
{
    public class LeaveRequestMappingProfiles : Profile
    {
        public LeaveRequestMappingProfiles()
        {
            CreateMap<LeaveRequestCreateDTO, LeaveRequest>();
        }
    }
}
