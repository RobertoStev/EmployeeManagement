using AutoMapper;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;

namespace EmployeeManagement.AutoMapper
{
    public class SickRequestMappingProfiles : Profile
    {
        public SickRequestMappingProfiles() 
        {
            CreateMap<SickLeave, SickLeaveGetDTO>();

            CreateMap<SickLeaveCreateDTO, SickLeave>();            
        }
    }
}
