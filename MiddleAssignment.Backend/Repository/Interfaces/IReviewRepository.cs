using MiddleAssignment.Backend.Models;

namespace MiddleAssignment.Backend.Repository.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviews();
        Task<Review> GetReviewById(Guid id);
        Task<IEnumerable<Review>> GetReviewsByBookId(Guid bookId);
        Task<Review> CreateReview(Review review);
        Task<Review> UpdateReview(Review review);
        Task DeleteReview(Guid id);
    }
}
