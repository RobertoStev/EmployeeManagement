using AutoMapper;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;

namespace EmployeeManagement.AutoMapper
{
    public class EmployeeMappingProfiles : Profile
    {
        public EmployeeMappingProfiles() 
        {
            CreateMap<Employee, EmployeeGetDTO>();

            CreateMap<EmployeeCreateDTO, Employee>();

            CreateMap<Employee, EmployeeManageDaysDTO>();

            CreateMap<Employee, EmployeeEditDTO>();

            CreateMap<EmployeeEditDTO, Employee>();           
        }
    }
}
