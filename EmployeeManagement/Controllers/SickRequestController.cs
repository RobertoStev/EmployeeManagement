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
    public class SickRequestController : Controller
    {
        private readonly ISickLeaveRequestRepository _sickLeaveRequestRepository;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IMemoryCache _cache;
        public SickRequestController(ISickLeaveRequestRepository sickLeaveRequestRepository, ISickLeaveService sickLeaveService, IMemoryCache cache) 
        { 
            _sickLeaveRequestRepository = sickLeaveRequestRepository;
            _sickLeaveService = sickLeaveService;    
            _cache = cache;
        }
        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> AllRequests()
        {
            string cacheKey = "AllSickLeaves";
            List<SickLeave> sickLeaves;
          
            if (!_cache.TryGetValue(cacheKey, out sickLeaves))
            {
                
                sickLeaves = await _sickLeaveRequestRepository.GetAllSickLeavesAndEmployeeAsync();
               
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), 
                    SlidingExpiration = TimeSpan.FromMinutes(5) 
                };

                //Save data in cache
                _cache.Set(cacheKey, sickLeaves, cacheOptions);
            }

            return View(sickLeaves);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int employeeId, SickLeaveCreateDTO sickLeave)
        {
            _cache.Remove("AllSickLeaves");

            if (ModelState.IsValid)
            { 
                await _sickLeaveService.CreateSickLeaveAsync(employeeId, sickLeave);
                return RedirectToAction("Requests", "Employee", new { id = employeeId });    
            }

            return View(sickLeave);
        }
        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Approve(int id)
        {
            _cache.Remove("AllSickLeaves");

            var success = await _sickLeaveService.ApproveSickLeaveAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("AllRequests");
        }
        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Decline(int id)
        {
            _cache.Remove("AllSickLeaves");

            var success = await _sickLeaveService.DeclineSickLeaveAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("AllRequests");
        }
    }

}
