using EmployeeManagement.Controllers;
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories;
using EmployeeManagement.Services;
using EmployeeManagement.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;

using static EmployeeManagement.Enums.EnumTypes;

namespace EmployeeManagement.Tests.Controllers
{
    [TestClass]
    public class LeaveRequestControllerTest
    {
        private ILeaveRequestRepository _fakeRepository;
        private ILeaveRequestService _fakeService;
        private IMemoryCache _mockCache;
        private LeaveRequestController _controller;

        [TestInitialize]
        public void Setup()
        {
            _fakeRepository = new FakeLeaveRequestRepository();
            _fakeService = new FakeLeaveRequestService();
            _mockCache = new MemoryCache(new MemoryCacheOptions());
            _controller = new LeaveRequestController(_fakeRepository, _fakeService, _mockCache);
        }

        [TestMethod]
        public async Task Create_ValidLeaveRequest_ShouldRedirectToRequests()
        {
            // Arrange
            var leaveRequestCreateDTO = new LeaveRequestCreateDTO
            {
                LeaveRequestId = 0, 
                StartDate = new DateTime(2025, 1, 10),
                EndDate = new DateTime(2025, 1, 15),
                Comment = "Family vacation",
                LeaveType = LeaveType.Annual,  // Assuming LeaveType.Annual is an enum value
                LeaveStatus = LeaveStatus.Pending  // Assuming LeaveStatus.Pending is an enum value
            };

            // Act
            var result = await _controller.Create(1, leaveRequestCreateDTO) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Requests", result.ActionName);  // Redirects to the "Requests" action in "Employee" controller
        }

        [TestMethod]
        public async Task Create_InvalidLeaveRequest_ShouldReturnViewWithModel()
        {
            // Arrange
            var leaveRequestCreateDTO = new LeaveRequestCreateDTO
            {
                StartDate = DateTime.MinValue, // Invalid start date
                EndDate = DateTime.MinValue,  // Invalid end date
                Comment = "",                 // Invalid comment
                LeaveType = LeaveType.Annual,
                LeaveStatus = LeaveStatus.Pending
            };

            _controller.ModelState.AddModelError("StartDate", "Start date is required");
            _controller.ModelState.AddModelError("EndDate", "End date is required");
            _controller.ModelState.AddModelError("Comment", "Comment is required");

            // Act
            var result = await _controller.Create(1, leaveRequestCreateDTO) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual(leaveRequestCreateDTO, result.Model); // Ensure the model is returned to the view
        }

        [TestMethod]
        public async Task Approve_ValidId_ShouldRedirectToAllRequests()
        {
            // Act
            var result = await _controller.Approve(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AllRequests", result.ActionName);  // Redirects to the "AllRequests" action
        }

        [TestMethod]
        public async Task Decline_ValidId_ShouldRedirectToAllRequests()
        {
            // Act
            var result = await _controller.Decline(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("AllRequests", result.ActionName);  // Redirects to the "AllRequests" action
        }
    }
}
