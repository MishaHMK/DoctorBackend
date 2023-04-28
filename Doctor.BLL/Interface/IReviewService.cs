using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.BLL.Interface
{
    public interface IReviewService
    {
        public Task<Review> Create(Review review);
        public Task DeleteReview(int Id);
        public Task<PagedList<ReviewDTO>> GetDoctorReviews(ReviewParams reviewParams, string Id);
        public Review GetReviewById(int Id);
        public Task<Review> EditReviewById(int id, Review model);
    }
}
