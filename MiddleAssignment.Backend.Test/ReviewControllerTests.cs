using System;
using Xunit;
using Moq;
using MiddleAssignment.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiddleAssignment.Backend.Services.Interfaces;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests
{
    public class ReviewControllerTests
    {
        private readonly Mock<IReviewService> _mockReviewService;
        private readonly ReviewController _reviewController;

        public ReviewControllerTests()
        {
            _mockReviewService = new Mock<IReviewService>();
            _reviewController = new ReviewController(_mockReviewService.Object);
        }

        [Fact]
        public async Task GetAllReviews_ShouldReturnOkResultWithReviews()
        {
            // Arrange
            var expectedReviews = new List<ReviewDTO>
            {
                new ReviewDTO { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 4, Comment = "Great book!" },
                new ReviewDTO { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 3, Comment = "Average read." }
            };
            _mockReviewService.Setup(s => s.GetAllReviews()).ReturnsAsync(expectedReviews);

            // Act
            var result = await _reviewController.GetAllReviews();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualReviews = Assert.IsAssignableFrom<IEnumerable<ReviewDTO>>(okResult.Value);
            Assert.Equal(expectedReviews, actualReviews);
        }

        [Fact]
        public async Task GetReviewById_WithValidId_ShouldReturnOkResultWithReview()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var expectedReview = new ReviewDTO { Id = reviewId, BookId = Guid.NewGuid(), UserId = Guid.NewGuid(), Rating = 4, Comment = "Great book!" };
            _mockReviewService.Setup(s => s.GetReviewById(reviewId)).ReturnsAsync(expectedReview);

            // Act
            var result = await _reviewController.GetReviewById(reviewId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualReview = Assert.IsType<ReviewDTO>(okResult.Value);
            Assert.Equal(expectedReview, actualReview);
        }

        [Fact]
        public async Task GetReviewById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _mockReviewService.Setup(s => s.GetReviewById(invalidId)).ReturnsAsync((ReviewDTO)null);

            // Act
            var result = await _reviewController.GetReviewById(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Add more tests for other controller actions
    }
}