using Doctor.BLL.Services;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using DoctorWebApi.Helper;
using Doctor.DataAcsess.Interfaces;
using Moq;
using Xunit;

namespace Doctor.XUnit
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _reviewRepository;

        public ReviewServiceTests()
        {
            _reviewRepository = new Mock<IReviewRepository>();
        }

        private ReviewService CreateReviewService()
        {
            _reviewRepository.Setup(r => r.GetReviewById(It.IsAny<int>()))
                .Returns(new DataAcsess.Entities.Review());
            _reviewRepository.Setup(r => r.GetPagedReviewOfDoctor(It.IsAny<ReviewParams>(), It.IsAny<string>()))
                           .ReturnsAsync(CreatePagedList);
            _reviewRepository.Setup(r => r.GetReviewByDocPat(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(new DataAcsess.Entities.Review());
            _reviewRepository.Setup(r => r.AddReview(It.IsAny<Review>()));
            _reviewRepository.Setup(r => r.DeleteReview(It.IsAny<Review>()));

            return new ReviewService(
                _reviewRepository.Object
            );
        }

        private static int GetIdForSearch => 1;

        [Fact]
        public async Task GetReviewById_ReturnsReview()
        {
            ReviewService reviewService = CreateReviewService();

            var result = reviewService.GetReviewById(GetIdForSearch);

            Assert.NotNull(result);
            Assert.IsType<Review>(result);
        }


        [Fact]
        public async Task GetDoctorReview_ReturnsReview()
        {
            ReviewService reviewService = CreateReviewService();
            var id = "29f40225-fc3b-4ee3-8758-baae8aaf4300";
            var reviewParams = new ReviewParams { PageNumber = 1, PageSize = 3 };

            var result = await reviewService.GetDoctorReviews(reviewParams, id);

            Assert.NotNull(result);
            Assert.IsType<PagedList<ReviewDTO>>(result);
        }


        [Fact]
        public async Task EditReviewById_ReturnsReview()
        {
            ReviewService reviewService = CreateReviewService();

            var result = await reviewService.EditReviewById(It.IsAny<int>(), new Review());

            Assert.NotNull(result);
            Assert.IsType<Review>(result);
        }


        [Fact]
        public async Task CreateReview_ReturnsReview()
        {
            ReviewService reviewService = CreateReviewService();

            var result = await reviewService.Create(new Review());

            Assert.NotNull(result);
            Assert.IsType<Review>(result);
        }



        private PagedList<ReviewDTO> CreatePagedList => new PagedList<ReviewDTO>(CreateReviewDTOEnumerable, 1, 1, CreateReviewDTOEnumerable.Count());

        private IEnumerable<ReviewDTO> CreateReviewDTOEnumerable => new List<ReviewDTO>()
        {
            new ReviewDTO(),
            new ReviewDTO()
        }.AsAsyncQueryable();
    }
}