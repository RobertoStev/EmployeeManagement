using AutoMapper;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public EmployeeController(IEmployeeRepository employeeRepository, IEmployeeService employeeService, IMapper mapper, IMemoryCache cache)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _mapper = mapper;
            _cache = cache;
        }

        public IActionResult TestRoles()
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            return Json(roles);
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> AllEmployees(int page = 1)
        {
            const string cacheKey = "AllEmployees";
            const int pageSize = 4;

            if (!_cache.TryGetValue(cacheKey, out List<Employee> employees))
            {
                employees = await _employeeRepository.GetAllEmployeesAsync();

                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache expires after 10 minutes
                    SlidingExpiration = TimeSpan.FromMinutes(5) // Cache expires if unused for 5 minutes
                };

                // Save data in cache
                _cache.Set(cacheKey, employees, cacheOptions);
            }

            var paginatedEmployees = employees
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var employeesGet = _mapper.Map<List<EmployeeGetDTO>>(paginatedEmployees);

            // Pass pagination info to the view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(employees.Count / (double)pageSize);

            return View(employeesGet);
        }

        public async Task<IActionResult> EmployeeInfo()
        {
            var userEmail = User.Identity.Name;
            if (userEmail == null)
            {
                return NotFound();
            }

            var loggedInUser = await _employeeRepository.GetEmployeeByEmailAsync(userEmail);

            // If the employee does not exist, return a message indicating HR needs to add them
            if (loggedInUser == null)
            {
                HttpContext.Session.Clear();
                ViewData["Message"] = "Successful login. Please wait until an HR employee adds you to their system.";
                return View();
            }

            // If the employee exists, store the EmployeeId in the session
            HttpContext.Session.SetString("EmployeeId", loggedInUser.EmployeeId.ToString());

            var employeeGet = _mapper.Map<EmployeeGetDTO>(loggedInUser);
            return View(employeeGet);
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Details(int id, int page)
        {
            var detailsForUser = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (detailsForUser == null)
            {
                return NotFound();
            }
       
            ViewBag.CurrentPage = page;
            var employeeGet = _mapper.Map<EmployeeGetDTO>(detailsForUser);

            return View(employeeGet);
        }

        public async Task<IActionResult> Requests(int id)
        {
            string? employeeId = HttpContext.Session.GetString("EmployeeId");
            if (string.IsNullOrEmpty(employeeId) || id.ToString() != employeeId)
            {
                return Redirect("/Identity/Account/AccessDenied");
            }

            var employee = await _employeeRepository.GetEmployeeWithRequestsAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeGet = _mapper.Map<EmployeeGetDTO>(employee);

            return View(employeeGet);
        }

        [Authorize(Roles = "Hr")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateDTO employee)
        {
            if (ModelState.IsValid)
            {
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                try
                {
                    await _employeeService.CreateEmployeeAsync(employee, imagesFolder);

                    _cache.Remove("AllEmployees");

                    return RedirectToAction("AllEmployees");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(employee);
                }     

            }
            return View(employee);
        }


        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Delete(int id, int page)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            
            await _employeeRepository.DeleteEmployeeByIdAsync(employee.EmployeeId);

            _cache.Remove("AllEmployees");

            _cache.Remove("AllLeaveRequests");

            _cache.Remove("AllSickLeaves");

            return RedirectToAction("AllEmployees", new { page = page });     
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> ManageDays(int id, int page)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.CurrentPage = page;
            var employeeManage = _mapper.Map<EmployeeManageDaysDTO>(employee);

            return View(employeeManage);
        }
        [HttpPost]
        public async Task<IActionResult> ManageDays(EmployeeManageDaysDTO employeeDto, int page)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.MangeDaysForEmployeeAsync(employeeDto);

                    _cache.Remove("AllEmployees");

                    return RedirectToAction("AllEmployees", new { page = page });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                    ViewBag.CurrentPage = page;

                    return View(employeeDto);
                }
            }
            return View(employeeDto);
        }

        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Edit(int id, int page)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.CurrentPage = page; // Pass the page to the view
            var editEmployee = _mapper.Map<EmployeeEditDTO>(employee);

            return View(editEmployee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeEditDTO employeeDto, int page)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.EditEmployeeAsync(employeeDto);

                    _cache.Remove("AllEmployees");
                    
                    _cache.Remove("AllLeaveRequests");
                   
                    _cache.Remove("AllSickLeaves");

                    return RedirectToAction("AllEmployees", new { page = page });
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(employeeDto);
                }                
            }
            return View(employeeDto);
        }
    }
}
