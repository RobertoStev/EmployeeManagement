using AutoMapper;
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
        private readonly IMapper _mapper;
        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepository, 
            ILeaveRequestService leaveRequestService, IMemoryCache cache, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveRequestService = leaveRequestService;
            _cache = cache;
            _mapper = mapper;
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> AllRequests(int page = 1, int pageSize = 5)
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

            var paginatedRequests = requests.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var requestsGet = _mapper.Map<List<LeaveRequestGetDTO>>(paginatedRequests);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRequests = requests.Count;

            return View(requestsGet);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int employeeId, LeaveRequestCreateDTO leaveRequest)
        { 
            if (ModelState.IsValid)
            {
                try
                {  
                    var leaveRequestToSave = await _leaveRequestService.CreateLeaveRequestAsync(employeeId, leaveRequest);

                    _cache.Remove("AllLeaveRequests");

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
        public async Task<IActionResult> Approve(int id, int page)
        {
            try
            {
                await _leaveRequestService.ApproveLeaveRequestAsync(id);

                _cache.Remove("AllLeaveRequests");

                return RedirectToAction("AllRequests", new { page = page });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("AllRequests", new { page = page });
            }

        }
        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Decline(int id, int page)
        {
            try
            {
                await _leaveRequestService.DeclineLeaveRequestAsync(id);

                _cache.Remove("AllLeaveRequests");

                return RedirectToAction("AllRequests", new { page = page });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);             
                return RedirectToAction("AllRequests", new { page = page });
            }
        }
    }
}
