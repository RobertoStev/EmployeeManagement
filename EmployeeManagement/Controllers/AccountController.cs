using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/RegisterNewUser
        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Register", model);

            // Create a new Employee
            var employee = new Employee
            {
                Name = model.EmployeeName,
                Email = model.Email
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Create a new ApplicationUser and link to the employee
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmployeeId = employee.EmployeeId // Link to the created employee
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee"); // Assign default role
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Register", model);
        }
    }
}
