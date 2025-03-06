using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeManagement.Controllers
{
    public class InfoController : Controller
    {
        private readonly ILogger<InfoController> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public InfoController(ILogger<InfoController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            // Redirect authenticated users to EmployeeInfo
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("EmployeeInfo", "Employee");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> HomePage()
        {
            var userEmail = User.Identity.Name;
            if (userEmail == null)
            {
                return NotFound();
            }

            var loggedInUser = await _employeeRepository.GetEmployeeByEmailAsync(userEmail);
            HttpContext.Session.SetString("EmployeeId", loggedInUser.EmployeeId.ToString());

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
