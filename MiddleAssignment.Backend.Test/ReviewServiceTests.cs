using System;
using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Implementations;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _mockReviewRepository;
        private readonly IMapper _mapper;
        private readonly ReviewService _reviewService;

        public ReviewServiceTests()
        {
            _mockReviewRepository = new Mock<IReviewRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Review, ReviewDTO>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _reviewService = new ReviewService(_mockReviewRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllReviews_ShouldReturnReviewDTOs()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 4, Comment = "Great book!" },
                new Review { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 3, Comment = "Average read." }
            };
            _mockReviewRepository.Setup(r => r.GetAllReviews()).ReturnsAsync(reviews);

            // Act
            var result = await _reviewService.GetAllReviews();

            // Assert
            Assert.Equal(reviews.Count, result.Count());
            // Add more assertions as needed
        }

        [Fact]
        public async Task GetReviewById_WithValidId_ShouldReturnReviewDTO()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var expectedReview = new Review { Id = reviewId, BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 4, Comment = "Great book!" };
            _mockReviewRepository.Setup(r => r.GetReviewById(reviewId)).ReturnsAsync(expectedReview);

            // Act
            var result = await _reviewService.GetReviewById(reviewId);

            // Assert
            Assert.Equal(expectedReview.Id, result.Id);
            Assert.Equal(expectedReview.BookId, result.BookId);
            Assert.Equal(expectedReview.UserId, result.UserId);
            Assert.Equal(expectedReview.Rating, result.Rating);
            Assert.Equal(expectedReview.Comment, result.Comment);
            // Add more assertions as needed
        }

        [Fact]
        public async Task GetReviewById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _mockReviewRepository.Setup(r => r.GetReviewById(invalidId)).ReturnsAsync((Review)null);

            // Act
            var result = await _reviewService.GetReviewById(invalidId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetReviewsByBookId_ShouldReturnReviewDTOs()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), BookId = bookId, UserId = Guid.NewGuid(), Rating = 4, Comment = "Great book!" },
                new Review { Id = Guid.NewGuid(), BookId = bookId, UserId = Guid.NewGuid(), Rating = 3, Comment = "Average read." }
            };
            _mockReviewRepository.Setup(r => r.GetReviewsByBookId(bookId)).ReturnsAsync(reviews);

            // Act
            var result = await _reviewService.GetReviewsByBookId(bookId);

            // Assert
            Assert.Equal(reviews.Count, result.Count());
            // Add more assertions as needed
        }

        [Fact]
        public async Task CreateReview_ShouldReturnCreatedReviewDTO()
        {
            // Arrange
            var reviewDTO = new ReviewDTO { BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 4, Comment = "Great book!" };
            var expectedReview = new Review { Id = Guid.NewGuid(), BookId = reviewDTO.BookId, UserId = reviewDTO.UserId, Rating = reviewDTO.Rating, Comment = reviewDTO.Comment };
            _mockReviewRepository.Setup(r => r.CreateReview(It.IsAny<Review>())).ReturnsAsync(expectedReview);

            // Act
            var result = await _reviewService.CreateReview(reviewDTO);

            // Assert
            Assert.Equal(expectedReview.Id, result.Id);
            Assert.Equal(expectedReview.BookId, result.BookId);
            Assert.Equal(expectedReview.UserId, result.UserId);
            Assert.Equal(expectedReview.Rating, result.Rating);
            Assert.Equal(expectedReview.Comment, result.Comment);
            // Add more assertions as needed
        }

        [Fact]
        public async Task UpdateReview_WithValidId_ShouldReturnUpdatedReviewDTO()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var existingReview = new Review { Id = reviewId, BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 3, Comment = "Average read." };
            var updatedReviewDTO = new ReviewDTO { Id = reviewId, BookId = existingReview.BookId, UserId = existingReview.UserId, Rating = 4, Comment = "Great book!" };
            var expectedReview = new Review { Id = reviewId, BookId = updatedReviewDTO.BookId, UserId = updatedReviewDTO.UserId, Rating = updatedReviewDTO.Rating, Comment = updatedReviewDTO.Comment };
            _mockReviewRepository.Setup(r => r.GetReviewById(reviewId)).ReturnsAsync(existingReview);
            _mockReviewRepository.Setup(r => r.UpdateReview(It.IsAny<Review>())).ReturnsAsync(expectedReview);

            // Act
            var result = await _reviewService.UpdateReview(reviewId, updatedReviewDTO);

            // Assert
            Assert.Equal(expectedReview.Id, result.Id);
            Assert.Equal(expectedReview.BookId, result.BookId);
            Assert.Equal(expectedReview.UserId, result.UserId);
            Assert.Equal(expectedReview.Rating, result.Rating);
            Assert.Equal(expectedReview.Comment, result.Comment);
            // Add more assertions as needed
        }

        [Fact]
        public async Task UpdateReview_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updatedReviewDTO = new ReviewDTO { Id = invalidId, BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 4, Comment = "Great book!" };
            _mockReviewRepository.Setup(r => r.GetReviewById(invalidId)).ReturnsAsync((Review)null);

            // Act
            var result = await _reviewService.UpdateReview(invalidId, updatedReviewDTO);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteReview_WithValidId_ShouldDeleteReview()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            _mockReviewRepository.Setup(r => r.GetReviewById(reviewId)).ReturnsAsync(new Review { Id = reviewId });

            // Act
            await _reviewService.DeleteReview(reviewId);

            // Assert
            _mockReviewRepository.Verify(r => r.DeleteReview(reviewId), Times.Once);
        }

        // Add more tests for other service methods
    }
}