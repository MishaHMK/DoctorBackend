using Doctor.BLL.Interface;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using DoctorWebApi.Controllers;
using DoctorWebApi.Helper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Doctor.NUnit.Controllers
{
    internal class ReviewControllerTests
    {
        private readonly Mock<IReviewService> _reviewService;

        public ReviewControllerTests()
        {
            _reviewService = new Mock<IReviewService>();
        }

        private ReviewController ReviewController =>
        new ReviewController(
            _reviewService.Object
        );

        [Test]
        public async Task GetReviewById_ReturnsOk()
        {
            // Arrange
            _reviewService.Setup(x => x.GetReviewById(It.IsAny<int>()))
                           .Returns(new DataAcsess.Entities.Review());

            // Act
            var result = await ReviewController.GetReviewById(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateReview_ReturnsOk()
        {
            // Arrange
            _reviewService.Setup(x => x.Create(It.IsAny<Review>()))
                           .ReturnsAsync(new DataAcsess.Entities.Review());

            // Act
            var result = await ReviewController.CreateReview(It.IsAny<Review>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateReview_ReturnsBadRequest()
        {
            // Arrange
            _reviewService.Setup(x => x.Create(It.IsAny<Review>()))
                           .ReturnsAsync(new DataAcsess.Entities.Review());

            var controller = ReviewController;
            controller.ModelState.AddModelError("NameError", "Required");

            // Act
            var result = await controller.CreateReview(It.IsAny<Review>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task EditReviewById_ReturnsOk()
        {
            // Arrange
            _reviewService.Setup(x => x.EditReviewById(It.IsAny<int>(), It.IsAny<Review>()))
                           .ReturnsAsync(new DataAcsess.Entities.Review());

            // Act
            var result = await ReviewController.EditReviewById(It.IsAny<int>(), It.IsAny<Review>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task EditReviewById_ReturnsNotFound()
        {
            // Arrange
            _reviewService.Setup(x => x.EditReviewById(It.IsAny<int>(), It.IsAny<Review>()))
                           .ReturnsAsync(It.IsAny<Review>());

            // Act
            var result = await ReviewController.EditReviewById(It.IsAny<int>(), It.IsAny<Review>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task DeleteReviewById_ReturnsOk()
        {
            // Arrange
            _reviewService.Setup(x => x.DeleteReview(It.IsAny<int>()));

            // Act
            var result = await ReviewController.DeleteReviewById(It.IsAny<int>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetMessagesForUser_ReturnsOk()
        {
            // Arrange
            var reviewParams = new ReviewParams { PageNumber = 1, PageSize = 3 };
            var id = "29f40225-fc3b-4ee3-8758-baae8aaf4300";
            var PagedDTOList = PagedList<ReviewDTO>.CreateAsync(RevDtoQueryable, reviewParams.PageNumber, reviewParams.PageSize, RevDtoQueryable.Count());

            _reviewService.Setup(x => x.GetDoctorReviews(reviewParams, id))
                       .Returns(PagedDTOList);

            // Act
            var result = await ReviewController.GetReviewsForDoctor(reviewParams, id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        private IQueryable<ReviewDTO> RevDtoQueryable => new List<ReviewDTO>()
        {
            new ReviewDTO(),
            new ReviewDTO()
        }.AsAsyncQueryable();


    }
}
