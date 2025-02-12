using AutoMapper;
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
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void Setup()
        { 
            _sickLeaveRequestRepository = new FakeSickLeaveRepository();
            _sickLeaveService = new FakeSickLeaveService();
            _memoryCache = new MemoryCache(new MemoryCacheOptions()); // Use a real cache but ignore it
            _mockMapper = new Mock<IMapper>();

            _controller = new SickRequestController(_sickLeaveRequestRepository, _sickLeaveService, _memoryCache, _mockMapper.Object);
        }

        [TestMethod]
        public async Task AllRequests_Returns_View_With_SickLeaves_When_Cache_Miss()
        {
            // Arrange
            var expectedSickLeaves = (await _sickLeaveRequestRepository.GetAllSickLeavesAndEmployeeAsync())
                .Take(5) // PageSize = 5 by default
                .ToList();

            // Mock AutoMapper configuration
            _mockMapper.Setup(m => m.Map<List<SickLeaveGetDTO>>(It.IsAny<List<SickLeave>>()))
                .Returns((List<SickLeave> source) =>
                    source.Select(s => new SickLeaveGetDTO
                    {
                        SickLeaveId = s.SickLeaveId,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        Reason = s.Reason,
                        MedicalReportPath = s.MedicalReportPath,
                        LeavStatus = s.LeavStatus,
                        EmployeeId = s.EmployeeId,
                        Employee = s.Employee                     
                    }).ToList());

            // Act
            var result = await _controller.AllRequests();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<SickLeaveGetDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(expectedSickLeaves.Count, model.Count);
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
            var result = await _controller.Approve(sickLeaveId, 1);

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
            var result = await _controller.Decline(sickLeaveId, 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("AllRequests", redirectResult.ActionName); // Ensure redirection to "AllRequests"
        }
    }
}
