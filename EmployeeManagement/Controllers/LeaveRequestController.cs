using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IMemoryCache _cache;
        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepository, 
            ILeaveRequestService leaveRequestService, IMemoryCache cache)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveRequestService = leaveRequestService;
            _cache = cache;
        }
        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> AllRequests()
        {
            const string cacheKey = "AllLeaveRequests";

            if (!_cache.TryGetValue(cacheKey, out List<LeaveRequest> requests))
            {
                requests = await _leaveRequestRepository.GetAllLeaveRequestsWithEmployeeAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };

                _cache.Set(cacheKey, requests, cacheOptions);
            }

            return View(requests);

        }


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int employeeId, LeaveRequestCreateDTO leaveRequest)
        {
            _cache.Remove("AllLeaveRequests");

            if (ModelState.IsValid)
            {
                try
                {  
                    var leaveRequestToSave = await _leaveRequestService.CreateLeaveRequestAsync(employeeId, leaveRequest);
                    return RedirectToAction("Requests", "Employee", new { id = employeeId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(leaveRequest);
                }
            }
            return View(leaveRequest);
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Approve(int id)
        {
            _cache.Remove("AllLeaveRequests");

            try
            {
                await _leaveRequestService.ApproveLeaveRequestAsync(id);
                return RedirectToAction("AllRequests");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("AllRequests");
            }

        }
        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Decline(int id)
        {
            _cache.Remove("AllLeaveRequests");

            try
            {
                await _leaveRequestService.DeclineLeaveRequestAsync(id);
                return RedirectToAction("AllRequests");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("AllRequests");
            }
        }
    }
}
