using AutoMapper;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Services
{
    public class SickLeaveService : ISickLeaveService
    {
        private readonly ISickLeaveRequestRepository _sickLeaveRepository;
        private readonly IMapper _mapper;
        public SickLeaveService(ISickLeaveRequestRepository sickLeaveRepository, IMapper mapper)
        {
            _sickLeaveRepository = sickLeaveRepository;
            _mapper = mapper;
        }

        
        public async Task CreateSickLeaveAsync(int employeeId, SickLeaveCreateDTO sickLeave)
        {
            string relativePath = null;

            if (sickLeave.MedicalReport != null && sickLeave.MedicalReport.Length > 0)
            {
                //Define the uploads folder inside wwwroot
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); //Ensure the folder exists

                //Generate a unique file name to avoid conflicts
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(sickLeave.MedicalReport.FileName);

                //Full file path for saving
                var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                //Save the file
                using (var stream = System.IO.File.Create(fullPath))
                {
                    await sickLeave.MedicalReport.CopyToAsync(stream);
                }

                // Save the relative path for later use
                relativePath = $"/uploads/{uniqueFileName}";
            }

            //Map DTO to entity and set additional properties
            var sickLeaveToSave = _mapper.Map<SickLeave>(sickLeave);
            sickLeaveToSave.MedicalReportPath = relativePath; //Use the relative path
            sickLeaveToSave.LeavStatus = Enums.EnumTypes.LeaveStatus.Pending;
            sickLeaveToSave.EmployeeId = employeeId;

            //Save to the database
            await _sickLeaveRepository.AddSickLeaveAsync(sickLeaveToSave);
        }
        public async Task<bool> ApproveSickLeaveAsync(int id)
        {
            var sickRequest = await _sickLeaveRepository.GetSickLeaveByIdAsync(id);
            if (sickRequest == null)
            {
                return false;
            }

            sickRequest.LeavStatus = Enums.EnumTypes.LeaveStatus.Approved;
            await _sickLeaveRepository.UpdateSickLeaveAsync(sickRequest);

            return true;
        }

        public async Task<bool> DeclineSickLeaveAsync(int id)
        { 
            var sickLeave = await _sickLeaveRepository.GetSickLeaveByIdAsync(id);
            if (sickLeave == null)
            {
                return false; 
            }

            sickLeave.LeavStatus = Enums.EnumTypes.LeaveStatus.Rejected;
            await _sickLeaveRepository.UpdateSickLeaveAsync(sickLeave);

            return true;         
        }
    }
}
