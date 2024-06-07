using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Backend.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetAllReviews();
        Task<ReviewDTO> GetReviewById(Guid id);
        Task<IEnumerable<ReviewDTO>> GetReviewsByBookId(Guid bookId);
        Task<ReviewDTO> CreateReview(ReviewDTO reviewDTO);
        Task<ReviewDTO> UpdateReview(Guid id, ReviewDTO reviewDTO);
        Task DeleteReview(Guid id);
    }
}
