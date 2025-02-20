using AutoMapper;
using EmployeeManagement.Controllers;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using EmployeeManagement.Services;
using EmployeeManagement.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;


namespace EmployeeManagement.Tests.Controllers
{
    [TestClass]
    public class EmployeeControllerTest
    {
        private IEmployeeRepository _fakeEmployeeRepository; 
        private IEmployeeService _fakeEmployeeService;
        private Mock<IMapper> _mockMapper;
        private IMemoryCache _mockCache;
        private EmployeeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _fakeEmployeeRepository = new FakeEmployeeRepository();
            _fakeEmployeeService = new FakeEmployeeService();
            _mockMapper = new Mock<IMapper>();
            _mockCache = new MemoryCache(new MemoryCacheOptions());
            _controller = new EmployeeController(_fakeEmployeeRepository, _fakeEmployeeService, _mockMapper.Object, _mockCache);

        }
        [TestMethod]
        public async Task AllEmployees_ReturnsViewWithEmployeeList()
        {
            var employees = await _fakeEmployeeRepository.GetAllEmployeesAsync();

            var employeeDTOs = employees.Select(e => new EmployeeGetDTO
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Department = e.Department,
                JobTitle = e.JobTitle,
                Email = e.Email,
                AnnualLeaveDaysRemaining = e.AnnualLeaveDaysRemaining,
                BonusLeaveDaysRemaining = e.BonusLeaveDaysRemaining,
                CreatedAt = e.CreatedAt
            }).ToList();

            // Setup the mock mapper to return the employeeDTOs
            _mockMapper.Setup(m => m.Map<List<EmployeeGetDTO>>(employees)).Returns(employeeDTOs);

            // Act: Call the controller action
            var result = await _controller.AllEmployees() as ViewResult;

            // Assert: Check that the result is a ViewResult and contains the correct model
            Assert.IsNotNull(result);

            var model = result.Model as List<EmployeeGetDTO>;

            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count); 
            Assert.AreEqual("John", model[0].FirstName);
            Assert.AreEqual("Doe", model[0].LastName);
            Assert.AreEqual(21, model[0].AnnualLeaveDaysRemaining); 
            Assert.AreEqual(21, model[1].AnnualLeaveDaysRemaining); 

            Assert.AreEqual(0, model[0].BonusLeaveDaysRemaining); 
            Assert.AreEqual(0, model[1].BonusLeaveDaysRemaining);
        }

        [TestMethod]
        public void Create_ReturnsView()
        {
            var result = _controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task Create_Post_ReturnsRedirectToAllEmployees_WhenModelIsValid()
        {
            // Arrange
            var employeeDto = new EmployeeCreateDTO { FirstName = "John", LastName = "Doe", Department ="IT", JobTitle = "Developer", Email = "john@example.com" };
       
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Department = "IT",
                JobTitle = "Developer",
                Email = "john@example.com",
                AnnualLeaveDaysRemaining = 21,
                BonusLeaveDaysRemaining = 0,
                CreatedAt = DateTime.Now,
                FirstPartLeaveExpiry = DateTime.Now.AddHours(1),
                SecondPartLeaveExpiry = DateTime.Now.AddHours(2),
            };

            _mockMapper.Setup(m => m.Map<Employee>(employeeDto)).Returns(employee);          

            // Act
            var result = await _controller.Create(employeeDto) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AllEmployees", result.ActionName);
        }

        [TestMethod]
        public async Task Create_Post_ReturnsView_WhenModelIsInvalid()
        {
            // Arrange
            var employeeDto = new EmployeeCreateDTO { FirstName = "", LastName = "", Department="", JobTitle="", Email = "" }; // Invalid model
            _controller.ModelState.AddModelError("FirstName", "First Name is required");
            _controller.ModelState.AddModelError("LastName", "Last Name is required");
            _controller.ModelState.AddModelError("Department", "Department is required");
            _controller.ModelState.AddModelError("JobTitle", "Job Title is required");

            // Act
            var result = await _controller.Create(employeeDto) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual(employeeDto, result.Model); 
        }

        [TestMethod]
        public async Task Delete_EmployeeNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var employeeId = 3;
            var pageNumber = 1;

            // Act
            var result = await _controller.Delete(employeeId, pageNumber);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
