using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Doctor.DataAcsess.Interfaces;


namespace Doctor.DataAcsess.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task AddReview(Review review)
        {
            await dbSet.AddAsync(review);
        }

        public async Task DeleteReview(Review reviewToDelete)
        {
            dbSet.Remove(reviewToDelete);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<PagedList<ReviewDTO>> GetPagedReviewOfDoctor(ReviewParams reviewParams, string Id)

        {
            var query = _db.Reviews.Where(x => x.DoctorId == Id)
                                 .Select(c => new ReviewDTO()
                                 {
                                    Id = c.Id,
                                    Description = c.Description,
                                    Score = c.Score,
                                    PostedOn = c.PostedOn,
                                    DoctorId = c.DoctorId,
                                    PatientId = c.PatientId,
                                    PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                                    DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()
                                 });

            return await PagedList<ReviewDTO>.CreateAsync(query, reviewParams.PageNumber, reviewParams.PageSize, query.Count());
        }

        public Review GetReviewById(int Id)
        {
            var review = _db.Reviews.Where(x => x.Id == Id)
                                  .Select(c => new Review()
                                  {

                                      Id = c.Id,
                                      Description = c.Description,
                                      Score = c.Score,
                                      PostedOn = c.PostedOn,
                                      DoctorId = c.DoctorId,
                                      PatientId = c.PatientId,
                              
                                  }).SingleOrDefault();

            return review;
        }


        public Review GetReviewByDocPat(string DoctorId, string PatientId)
        {
            var review = _db.Reviews.Where(x => x.DoctorId == DoctorId && x.PatientId == PatientId)
                                  .Select(c => new Review()
                                  {

                                      Id = c.Id,
                                      Description = c.Description,
                                      Score = c.Score,
                                      PostedOn = c.PostedOn,
                                      DoctorId = c.DoctorId,
                                      PatientId = c.PatientId
                                  }).SingleOrDefault();

            return review;
        }
    }
}
