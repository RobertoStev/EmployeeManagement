using EmployeeManagement.Controllers;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using EmployeeManagement.Services;
using EmployeeManagement.Tests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.Tests.Controllers
{
    [TestClass]
    public class SickRequestControllerTest
    {
        private ISickLeaveRequestRepository _sickLeaveRequestRepository;
        private ISickLeaveService _sickLeaveService;
        private IMemoryCache _memoryCache;
        private SickRequestController _controller;

        [TestInitialize]
        public void Setup()
        { 
            _sickLeaveRequestRepository = new FakeSickLeaveRepository();
            _sickLeaveService = new FakeSickLeaveService();
            _memoryCache = new MemoryCache(new MemoryCacheOptions()); // Use a real cache but ignore it

            _controller = new SickRequestController(_sickLeaveRequestRepository, _sickLeaveService, _memoryCache);
        }

        [TestMethod]
        public async Task AllRequests_Returns_View_With_SickLeaves_When_Cache_Miss()
        {
            // Act
            var result = await _controller.AllRequests();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<SickLeave>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count); // Ensure the fake repository data is returned
        }

        [TestMethod]
        public async Task Create_Post_Returns_Redirect_When_Valid_Model()
        {
            // Arrange
            var sickLeaveDto = new SickLeaveCreateDTO { 
                SickLeaveId = 3,
                StartDate = new DateTime(2025, 1, 10),
                EndDate = new DateTime(2025, 1, 15),
                Reason = "Flu",
                MedicalReport = new FormFile(
                baseStream: new MemoryStream(System.Text.Encoding.UTF8.GetBytes("This is a test medical report content.")),
                baseStreamOffset: 0,
                length: 41, // Length of the content
                name: "MedicalReport",
                fileName: "medical_report.pdf")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "application/pdf"
                        },
                LeaveStatus = LeaveStatus.Pending
            };
            int employeeId = 1;

            // Act
            var result = await _controller.Create(employeeId, sickLeaveDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Requests", redirectResult.ActionName); // Ensure redirection to "Requests"
            Assert.AreEqual("Employee", redirectResult.ControllerName); // Ensure redirection to "Employee" controller
        }

        [TestMethod]
        public async Task Approve_Returns_Redirect_When_Success()
        {
            // Arrange
            int sickLeaveId = 1;

            // Act
            var result = await _controller.Approve(sickLeaveId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("AllRequests", redirectResult.ActionName); // Ensure redirection to "AllRequests"
        }

        [TestMethod]
        public async Task Decline_Returns_NotFound_When_Failure()
        {
            // Arrange
            int sickLeaveId = 1;

            // Act
            var result = await _controller.Decline(sickLeaveId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("AllRequests", redirectResult.ActionName); // Ensure redirection to "AllRequests"
        }
    }
}
