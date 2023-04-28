using Doctor.BLL.Interface;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Doctor.DataAcsess.Interfaces;
using Doctor.DataAcsess.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace Doctor.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Review> Create(Review review)
        {
            Review newReview = new Review()
            {
                Description = review.Description,
                Score = review.Score,
                PostedOn = review.PostedOn,
                DoctorId = review.DoctorId,
                PatientId = review.PatientId
            };

            var existingReview = _reviewRepository.GetReviewByDocPat(review.DoctorId, review.PatientId);

            if (existingReview != null)
            {
                _reviewRepository.DeleteReview(existingReview); 
            }

            _reviewRepository.AddReview(newReview);

            await _reviewRepository.SaveAsync();

            return newReview;
        }


        public async Task<Review> EditReviewById(int id, Review model)
        {
            var reviewToUpdate = _reviewRepository.GetReviewById(id);

            if (reviewToUpdate == null)
            {
                return reviewToUpdate;
            }

            reviewToUpdate.Description = model.Description;
            reviewToUpdate.Score = model.Score;
            reviewToUpdate.PostedOn = model.PostedOn;
            reviewToUpdate.DoctorId = model.DoctorId;
            reviewToUpdate.PatientId = model.PatientId;


            await _reviewRepository.SaveAsync();

            return reviewToUpdate;
        }

        public async Task DeleteReview(int id)
        {
            var reviewToDelete = _reviewRepository.GetReviewById(id);
            await _reviewRepository.DeleteReview(reviewToDelete);
            await _reviewRepository.SaveAsync();
        }

        public async Task<PagedList<ReviewDTO>> GetDoctorReviews(ReviewParams reviewParams, string Id)
        {
            var reviews = await _reviewRepository.GetPagedReviewOfDoctor(reviewParams, Id);
            return reviews;
        }

        public Review GetReviewById(int Id)
        {
            var review = _reviewRepository.GetReviewById(Id);
            return review;
        }
    }
}
