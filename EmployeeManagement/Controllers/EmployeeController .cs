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
                // Cache miss: Fetch data from the repository
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

            // Paginate the employees
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
            if(userEmail == null)
            {
                return NotFound();
            }

            var loggedInUser = await _employeeRepository.GetEmployeeByEmailAsync(userEmail);

            // If the employee does not exist, return a message indicating HR needs to add them
            if (loggedInUser == null)
            {
                HttpContext.Session.Clear();
                ViewData["Message"] = "Successful login. Please wait until the HR employee add you to their system.";
                return View(); // Render the view with the message
            }

            // If the employee exists, store the EmployeeId in the session
            HttpContext.Session.SetString("EmployeeId", loggedInUser.EmployeeId.ToString());

            var employeeGet = _mapper.Map<EmployeeGetDTO>(loggedInUser);
            return View(employeeGet);
        }

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
                var domainEmployee = _mapper.Map<Employee>(employee);
                // Handle file upload
                if (employee.Picture != null && employee.Picture.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(employee.Picture.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Picture", "Only image files (JPG, PNG, GIF) are allowed");
                        return View(employee);
                    }

                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Generate filename
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(imagesFolder, fileName);
                    

                    // Save the file to the wwwroot/images folder
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await employee.Picture.CopyToAsync(stream);
                    }
                    
                    domainEmployee.Picture = fileName; // Save the file name in the database
                }
                else
                {
                    // If no picture is uploaded, set a default picture or leave it null
                    domainEmployee = _mapper.Map<Employee>(employee);
                    domainEmployee.Picture = "default.jpg"; // Default picture
                }

                await _employeeService.CreateEmployeeAsync(domainEmployee);

                // Invalidate the cache
                _cache.Remove("AllEmployees");

                return RedirectToAction("AllEmployees");
            }

            return View(employee);
        }


        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Delete(int id, int page)
        {
            //TODO: add confirmation for Deleting Employee!
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployeeByIdAsync(employee);

            _cache.Remove("AllEmployees");

            //Remove the LeaveRequests cache
            _cache.Remove("AllLeaveRequests");

            //Remove the SickLeaves cache
            _cache.Remove("AllSickLeaves");

            return RedirectToAction("AllEmployees", new { page = page }); // Redirect with page        }
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
            if(ModelState.IsValid)
            {
                try
                {
                    await _employeeService.MangeDaysForEmployeeAsync(employeeDto);

                    _cache.Remove("AllEmployees");

                    return RedirectToAction("AllEmployees", new { page = page }); // Redirect with page
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(employeeDto);
                }
            }
            return View(employeeDto);
        }


        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Edit(int id, int page)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if(employee == null)
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
                var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeDto.EmployeeId);
                if (employee == null)
                {
                    return NotFound();
                }

                _mapper.Map<Employee>(employeeDto);

                employee.Name = employeeDto.Name;

                await _employeeRepository.UpdateEmployeeAsync(employee);

                _cache.Remove("AllEmployees");

                return RedirectToAction("AllEmployees", new { page = page }); // Redirect with page
            }
            return View(employeeDto);
        }
    }
}
