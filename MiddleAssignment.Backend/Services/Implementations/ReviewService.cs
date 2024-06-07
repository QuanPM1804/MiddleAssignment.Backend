using AutoMapper;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Interfaces;

namespace MiddleAssignment.Backend.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllReviews()
        {
            var reviews = await _reviewRepository.GetAllReviews();
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> GetReviewById(Guid id)
        {
            var review = await _reviewRepository.GetReviewById(id);
            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByBookId(Guid bookId)
        {
            var reviews = await _reviewRepository.GetReviewsByBookId(bookId);
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> CreateReview(ReviewDTO reviewDTO)
        {
            var review = _mapper.Map<Review>(reviewDTO);
            review.ReviewDate = DateTime.UtcNow;
            var createdReview = await _reviewRepository.CreateReview(review);
            return _mapper.Map<ReviewDTO>(createdReview);
        }

        public async Task<ReviewDTO> UpdateReview(Guid id, ReviewDTO reviewDTO)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if (review == null)
                return null;

            _mapper.Map(reviewDTO, review);
            var updatedReview = await _reviewRepository.UpdateReview(review);
            return _mapper.Map<ReviewDTO>(updatedReview);
        }

        public async Task DeleteReview(Guid id)
        {
            await _reviewRepository.DeleteReview(id);
        }
    }
}
