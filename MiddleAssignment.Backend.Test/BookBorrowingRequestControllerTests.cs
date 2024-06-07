using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MiddleAssignment.Server.Controllers;
using Xunit;
using MiddleAssignment.Backend.Services.Interfaces;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests
{
    public class BookBorrowingRequestControllerTests
    {
        private readonly Mock<IBookBorrowingRequestService> _mockService;
        private readonly BookBorrowingRequestController _controller;
        private readonly Guid _userId = Guid.NewGuid();

        public BookBorrowingRequestControllerTests()
        {
            _mockService = new Mock<IBookBorrowingRequestService>();
            _controller = new BookBorrowingRequestController(_mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
                        }))
                    }
                }
            };
        }

        [Fact]
        public async Task GetAllRequests_ShouldReturnOkResultWithRequests()
        {
            // Arrange
            var requests = new List<BookBorrowingRequestDto>
            {
                new BookBorrowingRequestDto { Id = Guid.NewGuid(), RequestorId = _userId },
                new BookBorrowingRequestDto { Id = Guid.NewGuid(), RequestorId = _userId }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(requests);

            // Act
            var result = await _controller.GetAllRequests();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRequests = Assert.IsAssignableFrom<IEnumerable<BookBorrowingRequestDto>>(okResult.Value);
            Assert.Equal(requests, returnedRequests);
        }

        [Fact]
        public async Task GetRequestById_WithValidId_ShouldReturnOkResultWithRequest()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var request = new BookBorrowingRequestDto { Id = requestId, RequestorId = _userId };
            _mockService.Setup(s => s.GetByIdAsync(requestId)).ReturnsAsync(request);

            // Act
            var result = await _controller.GetRequestById(requestId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRequest = Assert.IsType<BookBorrowingRequestDto>(okResult.Value);
            Assert.Equal(request, returnedRequest);
        }

        [Fact]
        public async Task GetRequestById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(invalidId)).ReturnsAsync((BookBorrowingRequestDto)null);

            // Act
            var result = await _controller.GetRequestById(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateRequest_WithValidRequest_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var requestDTO = new BookBorrowingRequestDto
            {
                RequestorId = _userId,
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetailDto>()
            };
            var createdRequest = new BookBorrowingRequestDto { Id = Guid.NewGuid(), RequestorId = _userId };
            _mockService.Setup(s => s.AddAsync(requestDTO)).ReturnsAsync(createdRequest);

            // Act
            var result = await _controller.CreateRequest(requestDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedRequest = Assert.IsType<BookBorrowingRequestDto>(createdAtActionResult.Value);
            Assert.Equal(createdRequest, returnedRequest);
        }

        [Fact]
        public async Task CreateRequest_WithInvalidRequest_ShouldReturnBadRequestResult()
        {
            // Arrange
            var requestDTO = new BookBorrowingRequestDto
            {
                RequestorId = _userId,
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetailDto>()
            };
            _mockService.Setup(s => s.AddAsync(requestDTO)).ThrowsAsync(new InvalidOperationException("Error"));

            // Act
            var result = await _controller.CreateRequest(requestDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Error", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateRequest_WithValidRequest_ShouldReturnNoContentResult()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var requestDTO = new BookBorrowingRequestDto { Id = requestId, RequestorId = _userId };

            // Act
            var result = await _controller.UpdateRequest(requestId, requestDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateRequest_WithInvalidRequest_ShouldReturnBadRequestResult()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var requestDTO = new BookBorrowingRequestDto { Id = Guid.NewGuid(), RequestorId = _userId };

            // Act
            var result = await _controller.UpdateRequest(requestId, requestDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteRequest_WithValidId_ShouldReturnNoContentResult()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteRequest(requestId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ApproveRequest_WithValidId_ShouldReturnNoContentResult()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            // Act
            var result = await _controller.ApproveRequest(requestId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RejectRequest_WithValidId_ShouldReturnNoContentResult()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            // Act
            var result = await _controller.RejectRequest(requestId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}