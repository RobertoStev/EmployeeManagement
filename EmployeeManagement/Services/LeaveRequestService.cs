using AutoMapper;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Services
{
    
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

      
        public async Task<LeaveRequest> CreateLeaveRequestAsync(int employeeId, LeaveRequestCreateDTO leaveRequest)
        {

            leaveRequest.LeaveStatus = Enums.EnumTypes.LeaveStatus.Pending;

            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                return null; //Employee not found
            }

            var employeeAnnualDays = employee.AnnualLeaveDaysRemaining;
            var employeeBonusDays = employee.BonusLeaveDaysRemaining;
            int leaveDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

            if (leaveRequest.LeaveType == Enums.EnumTypes.LeaveType.Annual && employeeAnnualDays < leaveDays)
            {
                throw new Exception($"Not enough annual leave days. You have {employee.AnnualLeaveDaysRemaining} remaining.");
            }
            if (leaveRequest.LeaveType == Enums.EnumTypes.LeaveType.Bonus && employeeBonusDays < leaveDays)
            {
                throw new Exception($"Not enough bonus leave days. You have {employee.BonusLeaveDaysRemaining} remaining.");
            }

          
            var leaveRequestToSave = _mapper.Map<LeaveRequest>(leaveRequest);
            leaveRequestToSave.EmployeeId = employeeId;

            await _leaveRequestRepository.AddLeaveRequestAsync(leaveRequestToSave);


            return leaveRequestToSave;
        }

        public  async Task<bool> ApproveLeaveRequestAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestWithEmployeeAsync(id);
            if (leaveRequest == null)
            {
                throw new Exception("Leave request not found.");
            }

            int leaveDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;    

            if (leaveRequest.LeaveType == Enums.EnumTypes.LeaveType.Annual && leaveRequest.Employee.AnnualLeaveDaysRemaining < leaveDays)
            {
                throw new Exception($"Employee does not have enough annual leave days. " +
                $"Available: {leaveRequest.Employee.AnnualLeaveDaysRemaining}, Required: {leaveDays}");
            }

            if (leaveRequest.LeaveType == Enums.EnumTypes.LeaveType.Bonus && leaveRequest.Employee.BonusLeaveDaysRemaining < leaveDays)
            {
                throw new Exception($"Employee does not have enough bonus leave days. " +
                $"Available: {leaveRequest.Employee.BonusLeaveDaysRemaining}, Required: {leaveDays}");
            }

            // Deduct the leave days from the appropriate type
            if (leaveRequest.LeaveType == Enums.EnumTypes.LeaveType.Annual)
            {
                leaveRequest.Employee.AnnualLeaveDaysRemaining -= leaveDays;
            }
            else if (leaveRequest.LeaveType == Enums.EnumTypes.LeaveType.Bonus)
            {
                leaveRequest.Employee.BonusLeaveDaysRemaining -= leaveDays;
            }

            leaveRequest.LeaveStatus = Enums.EnumTypes.LeaveStatus.Approved;

            // Save the leave request and update employee data
            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);

            return true;
        }

        public async Task<bool> DeclineLeaveRequestAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestsByIdAsync(id);
            if (leaveRequest == null)
            {
                throw new Exception("Leave request not found.");
            }

            leaveRequest.LeaveStatus = Enums.EnumTypes.LeaveStatus.Rejected;

            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);

            return true;
        }
    }
}
