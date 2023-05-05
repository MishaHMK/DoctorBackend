using Doctor.BLL.Interface;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity.UI.Services;
using Doctor.DataAcsess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using DoctorWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DoctorWebApi.Helper;
using System.Web.WebPages;
using DocumentFormat.OpenXml.Wordprocessing;
using Doctor.DataAcsess.Helpers;
using System.Data;

namespace Doctor.NUnit.Controllers
{
    [TestFixture]
    internal class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _appointService;
        private readonly Mock<IExportUtility> _exportUtility;

        public AppointmentControllerTests()
        {
            _appointService = new Mock<IAppointmentService>();  
            _exportUtility = new Mock<IExportUtility>();    
        }




        private AppointmentController AppointmentController => 
        new AppointmentController(
            _appointService.Object,
            _exportUtility.Object
        );


        [Test]
        public async Task GetDoctors_ReturnsOk()
        {
            // Arrange
            _appointService.Setup(x => x.GetDoctorList())
                           .Returns(new List<Doctor.DataAcsess.Entities.Doctor>());

            // Act
            var result = await AppointmentController.GetDoctors();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetPatients_ReturnsOk()
        {
            // Arrange
            _appointService.Setup(x => x.GetPatientList())
                           .Returns(new List<Patient>());

            // Act
            var result = await AppointmentController.GetPatients();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task SaveData_ReturnsOk()
        {
            // Arrange
            var testModel = new AppointmentDTO() 
            { 
                Title = "Test", 
                Description = "Some details",
                StartDate = "2023-03-03 14:20",
                EndDate = "2023-03-03 14:40",
                Duration = 0,
                DoctorId = "29f40225-fc3b-4ee3-8758-baae8aaf4300",
                PatientId = "6e49721c-5333-4913-99c1-6e73e38bfbc3",

            };

            _appointService.Setup(x => x.Add(testModel))
                           .ReturnsAsync(new Appointment());

            // Act
            var result = await AppointmentController.SaveData(testModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task SaveData_ReturnsBadRequest()
        {
            // Arrange
            var testModel = new AppointmentDTO()
            {
                Title = null,
                Description = "Some details",
                StartDate = "2023-03-03 14:20",
                EndDate = "2023-03-03 14:40",
                Duration = 0,
                DoctorId = "29f40225-fc3b-4ee3-8758-baae8aaf4300",
                PatientId = "6e49721c-5333-4913-99c1-6e73e38bfbc3",

            };

            _appointService.Setup(x => x.Add(testModel))
                           .ReturnsAsync(new Appointment());

            var controller = AppointmentController;
            controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await controller.SaveData(testModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

       [Test]
        public async Task GetPagedApps_ReturnsOk()
        {
            // Arrange
            _appointService.Setup(x => x.GetUserAppoints(It.IsAny<AppointParams>(), It.IsAny<string>()))
                           .ReturnsAsync(CreatePagedList);

            // Act
            var result = await AppointmentController.GetApps(CreateAppointParams, It.IsAny<string>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetPagedApps_ReturnsBadRequest()
        {
            // Arrange
            _appointService.Setup(x => x.GetUserAppoints(It.IsAny<AppointParams>(), It.IsAny<string>()))
                           .ReturnsAsync(It.IsAny<PagedList<AppointmentDTOPage>>());



            // Act
            var result = await AppointmentController.GetApps(It.IsAny<AppointParams>(), It.IsAny<string>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }



        [Test]
        public async Task GetCallendarData_ForPatients_ReturnsOk()
        {
            // Arrange
            _appointService.Setup(x => x.PatientsEventsById(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(It.IsAny<List<AppointmentDTO>>());

            // Act
            var result = await AppointmentController.GetCalendarData("Patient", It.IsAny<string>(), It.IsAny<string>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetCallendarData_ForDoctors_ReturnsOk()
        {
            // Arrange
            _appointService.Setup(x => x.DoctorEventsById(It.IsAny<string>()))
                           .Returns(It.IsAny<List<AppointmentDTO>>());

            // Act
            var result = await AppointmentController.GetCalendarData("Doctor", It.IsAny<string>(), It.IsAny<string>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCallendarData_OnException_ReturnsBadRequest()
        {
            // Arrange
            var controller = AppointmentController;
            _appointService.Setup(x => x.DoctorEventsById(It.IsAny<string>())).Throws(new NullReferenceException());


            // Act
            var result = await AppointmentController.GetCalendarData("", It.IsAny<string>(), It.IsAny<string>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }


        [Test]
        public async Task GetCalendarDataById_ReturnsOk()
        {
            // Arrange
            _appointService.Setup(x => x.GetDetailsById(It.IsAny<int>()))
                           .ReturnsAsync(It.IsAny<AppointmentDTO>());

            // Act
            var result = await AppointmentController.GetCalendarDataById(It.IsAny<int>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCalendarDataById_OnException_ReturnsBadRequest()
        {
            // Arrange
            var controller = AppointmentController;
            _appointService.Setup(x => x.GetDetailsById(It.IsAny<int>())).Throws(new ArgumentException());


            // Act
            var result = await AppointmentController.GetCalendarDataById(It.IsAny<int>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }


        [Test]
        public async Task EditAppointmentById_ReturnsNotFound()
        {
            // Arrange
            _appointService.Setup(x => x.EditAppointmentById(It.IsAny<int>(), It.IsAny<AppointmentDTO>()))
                           .ReturnsAsync(It.IsAny<Appointment>());

            // Act
            var result = await AppointmentController.EditAppointmentById(It.IsAny<int>(), It.IsAny<AppointmentDTO>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task EditAppointmentById_ReturnsOk()
        {
            // Arrange
            var testModel = new AppointmentDTO() { Id = 1 };
            var testApp = new Appointment();
            _appointService.Setup(x => x.EditAppointmentById(It.IsAny<int>(), It.IsAny<AppointmentDTO>()))
                           .ReturnsAsync(testApp);
            var controller = AppointmentController;

            // Act
            var result = await controller.EditAppointmentById(It.IsAny<int>(), testModel);

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task ApproveAppointmentById_ReturnsNotFound()
        {
            // Arrange
            _appointService.Setup(x => x.ApproveAppointmentById(It.IsAny<int>(), It.IsAny<bool>()))
                           .ReturnsAsync(It.IsAny<Appointment>());

            // Act
            var result = await AppointmentController.ApproveAppointmentById(It.IsAny<int>(), It.IsAny<bool>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task ApproveAppointmentById_ReturnsOk()
        {
            // Arrange
            var testModel = new AppointmentDTO() { Id = 1 };
            var testApp = new Appointment();
            _appointService.Setup(x => x.ApproveAppointmentById(It.IsAny<int>(), It.IsAny<bool>()))
                           .ReturnsAsync(testApp);
            var controller = AppointmentController;

            // Act
            var result = await AppointmentController.ApproveAppointmentById(It.IsAny<int>(), It.IsAny<bool>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task DeleteAppointmentById_ReturnsOk()
        {
            // Arrange
            _appointService.Setup(x => x.DeleteAppointmentById(It.IsAny<int>()));

            var controller = AppointmentController;

            // Act
            var result = await AppointmentController.DeleteAppointmentById(It.IsAny<int>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetReport_ReturnsOk()
        {
            // Arrange
            _appointService.Setup(x => x.GetAppointmentsReview(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                           .ReturnsAsync(CreateReportList);

            var controller = AppointmentController;


            // Act
            var result = await AppointmentController.GetReport(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());

            // Assert
            _appointService.Verify();
            Assert.NotNull(result);
            Assert.IsInstanceOf<DataTable>(result);
        }


        private AppointParams CreateAppointParams => new AppointParams()
        {
            PageSize = 10,
            CurrentRole = "Doctor"
        };

       private PagedList<AppointmentDTOPage> CreatePagedList => new PagedList<AppointmentDTOPage>(CreateAppointmentDTOList, 1, 1, 1);

        private List<ReportAppointment> CreateReportList => new List<ReportAppointment>();

        private IEnumerable<AppointmentDTOPage> CreateAppointmentDTOList => new List<AppointmentDTOPage>()
        {
            new AppointmentDTOPage(),
            new AppointmentDTOPage()
        };
    }



}
