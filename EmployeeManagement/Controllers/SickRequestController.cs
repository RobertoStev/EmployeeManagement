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
    public class SickRequestController : Controller
    {
        private readonly ISickLeaveRequestRepository _sickLeaveRequestRepository;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        public SickRequestController(ISickLeaveRequestRepository sickLeaveRequestRepository, ISickLeaveService sickLeaveService, IMemoryCache cache, IMapper mapper) 
        { 
            _sickLeaveRequestRepository = sickLeaveRequestRepository;
            _sickLeaveService = sickLeaveService;    
            _cache = cache;
            _mapper = mapper;
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> AllRequests(int page = 1, int pageSize = 5)
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

                _cache.Set(cacheKey, sickLeaves, cacheOptions);
            }

            var paginatedSickLeaves = sickLeaves
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToList();

            var sickLeavesGet = _mapper.Map<List<SickLeaveGetDTO>>(paginatedSickLeaves);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRequests = sickLeaves.Count;   

            return View(sickLeavesGet);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int employeeId, SickLeaveCreateDTO sickLeave)
        {
            if (ModelState.IsValid)
            { 
                await _sickLeaveService.CreateSickLeaveAsync(employeeId, sickLeave);

                _cache.Remove("AllSickLeaves");

                return RedirectToAction("Requests", "Employee", new { id = employeeId });    
            }

            return View(sickLeave);
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Approve(int id, int page)
        {
            await _sickLeaveService.ApproveSickLeaveAsync(id);
            _cache.Remove("AllSickLeaves");

            return RedirectToAction("AllRequests", new { page = page });
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Decline(int id, int page)
        {
            var success = await _sickLeaveService.DeclineSickLeaveAsync(id);
            if (!success)
            {
                return NotFound();
            }

            _cache.Remove("AllSickLeaves");

            return RedirectToAction("AllRequests", new { page = page });
        }
    }
}
