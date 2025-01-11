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
        public async Task<IActionResult> AllEmployees()
        {   
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            var employeesGet = _mapper.Map<List<EmployeeGetDTO>>(employees);
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

        public async Task<IActionResult> Details(int id)
        {
            var detailsForUser = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (detailsForUser == null)
            {
                return NotFound();
            }
  
            return View(detailsForUser);
        }

        public async Task<IActionResult> Requests(int id)
        {
            var employee = await _employeeRepository.GetEmployeeWithRequestsAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }



        [Authorize(Roles = "Hr")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateDTO employee)
        {
            if(ModelState.IsValid)
            {
                var domainEmployee = _mapper.Map<Employee>(employee);

                await _employeeService.CreateEmployeeAsync(domainEmployee);

                return RedirectToAction("AllEmployees");
            }
            return View(employee);
        }



        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteEmployeeByIdAsync(employee);

            return RedirectToAction("AllEmployees");
        }



        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> ManageDays(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeManage = _mapper.Map<EmployeeManageDaysDTO>(employee);
            return View(employeeManage);
        }
        [HttpPost]
        public async Task<IActionResult> ManageDays(EmployeeManageDaysDTO employeeDto)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _employeeService.MangeDaysForEmployeeAsync(employeeDto);
                    return RedirectToAction("AllEmployees");
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }

            }
            return View(employeeDto);
        }


        [Authorize(Roles = "Hr")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if(employee == null)
            {
                return NotFound();
            }

            var editEmployee = _mapper.Map<EmployeeEditDTO>(employee);
            return View(editEmployee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeEditDTO employeeDto)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(employeeDto);
                try
                {
                    await _employeeRepository.UpdateEmployeeAsync(employee);
                    return RedirectToAction("AllEmployees");
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }

            }
            return View(employeeDto);
        }
    }
}
