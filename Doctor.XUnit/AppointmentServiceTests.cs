using Doctor.BLL.Services;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Doctor.DataAcsess.Interfaces;
using DocumentFormat.OpenXml.Office2013.Word;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using Xunit;

namespace Doctor.XUnit
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepository;
        private readonly Mock<IEmailSender>  _emailSender;

        public AppointmentServiceTests()
        {
            _appointmentRepository = new Mock<IAppointmentRepository>();
            _emailSender = new Mock<IEmailSender>();
        }

        private AppointmentService CreateAppointmentService()
        {
            _appointmentRepository.Setup(r => r.AddAppointment(new Appointment()));

            _appointmentRepository.Setup(r => r.SaveAsync());

            _appointmentRepository.Setup(r => r.DoctorEventsById(It.IsAny<string>()))
                           .Returns(CreateAppointmentDTOList);

            _appointmentRepository.Setup(r => r.PatientEventsById(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(CreateAppointmentDTOList);

            _appointmentRepository.Setup(r => r.GetReportList(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                            .ReturnsAsync(CreateReportAppointmentList);

            _appointmentRepository.Setup(r => r.GetDetailsById(It.IsAny<int>()))
                           .Returns(new AppointmentDTO());

            _appointmentRepository.Setup(r => r.GetUser(It.IsAny<string>()))
                            .Returns(new User());

            _appointmentRepository.Setup(r => r.GetAppointment(It.IsAny<int>()))
                            .Returns(new Appointment());

            _appointmentRepository.Setup(r => r.GetDoctorList())
                            .Returns(CreateDoctorList);

            _appointmentRepository.Setup(r => r.GetPatientList())
                            .Returns(CreatePatientList);

            _appointmentRepository.Setup(r => r.GetPagedAppointsForDoctor(CreateAppointParams, It.IsAny<string>()))
                            .ReturnsAsync(CreatePagedList);

            _appointmentRepository.Setup(r => r.GetPagedAppointsForPatient(CreateAppointParams, It.IsAny<string>()))
                            .ReturnsAsync(CreatePagedList);

            _appointmentRepository.Setup(r => r.GetApointmentDateList())
                            .Returns(CreateAppointmentDateList);

            _appointmentRepository.Setup(r => r.GetPagedAppointsForDoctor(It.IsAny<AppointParams>(), It.IsAny<string>()))
                            .ReturnsAsync(CreatePagedList);

            _appointmentRepository.Setup(r => r.GetPagedAppointsForPatient(It.IsAny<AppointParams>(), It.IsAny<string>()))
                            .ReturnsAsync(CreatePagedList);

            _appointmentRepository.Setup(r => r.GetApointmentDateList())
                            .Returns(CreateAppointmentDateList);

            _appointmentRepository.Setup(r => r.DeleteAppointments(new Appointment()));


            return new AppointmentService(
                _appointmentRepository.Object,
                _emailSender.Object
            );
        }

        [Fact]
        public async Task AddAppointment_ReturnsAppointment()
        {
            AppointmentService appService = CreateAppointmentService();

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

            var result = await appService.Add(testModel);

            Assert.NotNull(result);
            Assert.IsType<Appointment>(result);
        }


        [Fact]
        public async Task DoctorEventsById_ReturnsAppointmentDTOList()
        {
            AppointmentService appService = CreateAppointmentService();

            var result = appService.DoctorEventsById(It.IsAny<string>());

            Assert.NotNull(result);
            Assert.IsType<List<AppointmentDTO>>(result);
        }

        [Fact]
        public async Task PatientEventsById_ReturnsAppointmentDTOList()
        {
            AppointmentService appService = CreateAppointmentService();

            var result = appService.PatientsEventsById(It.IsAny<string>(), It.IsAny<string>());

            Assert.NotNull(result);
            Assert.IsType<List<AppointmentDTO>>(result);
        }

        [Fact]
        public async Task GetApointmentDateList_ReturnsAppointmentDateList()
        {
            AppointmentService appService = CreateAppointmentService();

            var result = appService.GetApointmentDateList();

            Assert.NotNull(result);
            Assert.IsType<List<AppointmentDate>>(result);
        }

        [Fact]
        public async Task GetAppointmentsReport_ReturnsAppointmentRepList()
        {
            AppointmentService appService = CreateAppointmentService();

            var result = await appService.GetAppointmentsReview(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());

            Assert.NotNull(result);
            Assert.IsType<List<ReportAppointment>>(result);
        }


        [Fact]
        public async Task GetDetailsById_ReturnsAppointmentDTO()
        {
            AppointmentService appService = CreateAppointmentService();

            var result = await appService.GetDetailsById(It.IsAny<int>());

            Assert.NotNull(result);
            Assert.IsType<AppointmentDTO>(result);
        }

        [Fact]
        public async Task GetDoctorList_ListDoctor()
        {
            AppointmentService appService = CreateAppointmentService();

            var result = appService.GetDoctorList();

            Assert.NotNull(result);
            Assert.IsType<List<Doctor.DataAcsess.Entities.Doctor>>(result);
        }

        [Fact]
        public async Task GetPatientList_ListPatient()
        {
            AppointmentService appService = CreateAppointmentService();

            var result = appService.GetPatientList();

            Assert.NotNull(result);
            Assert.IsType<List<Patient>>(result);
        }

        [Fact]
        public async Task GetUserAppoints_PagedList()
        {
            AppointmentService appService = CreateAppointmentService();
            var id = "29f40225-fc3b-4ee3-8758-baae8aaf4300";    

            var result = await appService.GetUserAppoints(CreateAppointParams, id);

            Assert.NotNull(result);
            Assert.IsType<PagedList<AppointmentDTOPage>>(result);
        }

        [Fact]
        public async Task EditAppointmentById_ReturnsAppointment()
        {
            AppointmentService appService = CreateAppointmentService();
            var id = "29f40225-fc3b-4ee3-8758-baae8aaf4300";

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

            var result = await appService.EditAppointmentById(It.IsAny<int>(), testModel);

            Assert.NotNull(result);
            Assert.IsType<Appointment>(result);
        }

        private List<Doctor.DataAcsess.Entities.Doctor> CreateDoctorList => new List<Doctor.DataAcsess.Entities.Doctor>();
        private List<Patient> CreatePatientList => new List<Patient>();

        private PagedList<AppointmentDTOPage> CreatePagedList => new PagedList<AppointmentDTOPage>(CreateAppointmentDTOEnumer, 1, 1, 1);

        private List<AppointmentDTO> CreateAppointmentDTOList => new List<AppointmentDTO>();

        private List<AppointmentDate> CreateAppointmentDateList => new List<AppointmentDate>();

        private List<ReportAppointment> CreateReportAppointmentList => new List<ReportAppointment>();

        private IEnumerable<AppointmentDTOPage> CreateAppointmentDTOEnumer => new List<AppointmentDTOPage>()
        {
            new AppointmentDTOPage(),
            new AppointmentDTOPage()
        };

        private AppointParams CreateAppointParams => new AppointParams()
        {
            PageSize = 10,
            CurrentRole = "Doctor"
        };

    }
}
