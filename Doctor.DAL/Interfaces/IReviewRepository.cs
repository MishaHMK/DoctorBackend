using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.DataAcsess.Interfaces
{
    public interface IReviewRepository
    {
        public Task AddReview(Review review);
        public Task DeleteReview(Review reviewToDelete);
        public Task SaveAsync();
        public Task<PagedList<ReviewDTO>> GetPagedReviewOfDoctor(ReviewParams reviewParams, string Id);
        public Review GetReviewById(int Id);
        public Review GetReviewByDocPat(string DoctorId, string PatientId);
    }
}
