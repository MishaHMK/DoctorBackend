using AutoMapper;
using Doctor.BLL.Interface;
using Doctor.DataAcsess;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using DoctorWebApi.Controllers;
using DoctorWebApi.Helpers;
using DocumentFormat.OpenXml.Spreadsheet;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Web;
using User = Doctor.DataAcsess.Entities.User;

namespace Doctor.NUnit.Controllers
{
    internal class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accService;
        private readonly Mock<ApplicationDbContext> _db;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<IJWTService> _jWTManager;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<SignInManager<User>> _signInManager;
        private Mock<IHttpContextAccessor> _contextAccessor;
        private Mock<IUserClaimsPrincipalFactory<User>> _principalFactory;

        private HostUrlOptions _hostUrl;
        private Mock<IOptions<HostUrlOptions>> options;

        public AccountControllerTests()
        {
            _accService = new Mock<IAccountService>();  
            _emailSender = new Mock<IEmailSender>();
            _jWTManager = new Mock<IJWTService>();
            _userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _principalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signInManager = new Mock<SignInManager<User>>(_userManager.Object,
                           _contextAccessor.Object, _principalFactory.Object, null, null, null, null);
            _hostUrl = new HostUrlOptions();
            options = new Mock<IOptions<HostUrlOptions>>();
           // _db = new Mock<ApplicationDbContext>();
        }

        private AccountController AccountController =>
        new AccountController(
          //  _db.Object,
           _userManager.Object,
           _signInManager.Object,
           _emailSender.Object,
           _accService.Object,
           _jWTManager.Object,
           options.Object
        );

        [Test]
        public async Task CheckRoles_ReturnsOk()
        {
            // Arrange
            _accService.Setup(x => x.GetRoles())
                           .ReturnsAsync(new List<string>());

            // Act
            var result = await AccountController.CheckRoles();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetTimes_ReturnsOk()
        {
            // Arrange
            _accService.Setup(x => x.GetTimes())
                           .ReturnsAsync(new List<string>());

            // Act
            var result = await AccountController.GetTimes();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetSpecialities_ReturnsOk()
        {
            // Arrange
            _accService.Setup(x => x.GetSpecialities())
                           .ReturnsAsync(new List<string>());

            // Act
            var result = await AccountController.GetSpecialities();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ConfirmEmail_ReturnsOk()
        {
            // Arrange
            var decodedToken = HttpUtility.UrlDecode("token");

            var user = new DataAcsess.Entities.User();   

            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                          .ReturnsAsync(new User());

            _userManager.Setup(x => x.ConfirmEmailAsync(user, decodedToken))
                           .ReturnsAsync(new IdentityResult());

            // Act
            var result = await AccountController.ConfirmEmail(It.IsAny<string>(), decodedToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ConfirmEmail_ReturnsNotFound()
        {
            // Arrange
            var decodedToken = HttpUtility.UrlDecode("token");

            var user = new DataAcsess.Entities.User();

            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                          .ReturnsAsync((User)null);

            _userManager.Setup(x => x.ConfirmEmailAsync(user, decodedToken))
                           .ReturnsAsync(new IdentityResult());

            // Act
            var result = await AccountController.ConfirmEmail(It.IsAny<string>(), decodedToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task EditAccountById_ReturnsOk()
        {
            // Arrange
            _accService.Setup(x => x.GetUserById(It.IsAny<string>()))
                          .ReturnsAsync(new User());

            // Act
            var result = await AccountController.EditAccountById(It.IsAny<string>(), CreateEditUserForm);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditAccountById_ReturnsNotFound()
        {
            // Arrange
            _accService.Setup(x => x.GetUserById(It.IsAny<string>()))
                          .ReturnsAsync((User)null);

            // Act
            var result = await AccountController.EditAccountById(It.IsAny<string>(), CreateEditUserForm);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetUsers_ReturnsOk()
        {
            // Arrange
            _accService.Setup(x => x.GetAllUsers())
                          .ReturnsAsync(CreateAllUsersList);

            // Act
            var result = await AccountController.GetAllUsers();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetUserById_ReturnsOk()
        {
            // Arrange
            _accService.Setup(x => x.GetUserDTOById(It.IsAny<string>()))
                          .ReturnsAsync(new UserDTO());

            // Act
            var result = await AccountController.GetUserById(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        private EditUserForm CreateEditUserForm => new EditUserForm()
        {
            Name = "Mykhailo",
            Fathername = "Igorovych",
            Surname = "Humeniuk"
        };

        private UserParams CreateUserParams => new UserParams()
        {
            PageSize = 10,
            PageNumber = 1,
            SearchName = "Mykhailo",
            Speciality = "Any",
            OrderBy = "ascend"
        };

        private PagedList<UserRateDTO> CreatePagedList => new PagedList<UserRateDTO>(CreateUserList, 1, 1, 1);
        private List<UserRateDTO> CreateUserList => new List<UserRateDTO>();
        private List<User> CreateAllUsersList => new List<User>();
    }
}
