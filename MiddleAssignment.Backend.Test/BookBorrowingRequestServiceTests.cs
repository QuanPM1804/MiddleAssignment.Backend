using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Implementations;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests
{
    public class BookBorrowingRequestServiceTests
    {
        private readonly Mock<IBookBorrowingRequestRepository> _mockRepository;
        private readonly IMapper _mapper;
        private readonly BookBorrowingRequestService _service;
        private readonly Guid _userId = Guid.NewGuid();

        public BookBorrowingRequestServiceTests()
        {
            _mockRepository = new Mock<IBookBorrowingRequestRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BookBorrowingRequest, BookBorrowingRequestDto>().ReverseMap();
                cfg.CreateMap<BookBorrowingRequestDetail, BookBorrowingRequestDetailDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _service = new BookBorrowingRequestService(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnRequestDtos()
        {
            // Arrange
            var requests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = Guid.NewGuid(), RequestorId = _userId },
                new BookBorrowingRequest { Id = Guid.NewGuid(), RequestorId = _userId }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(requests);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(requests.Count, result.Count());
            // Add more assertions as needed
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnRequestDto()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var request = new BookBorrowingRequest { Id = requestId, RequestorId = _userId };
            _mockRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(request);

            // Act
            var result = await _service.GetByIdAsync(requestId);

            // Assert
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.RequestorId, result.RequestorId);
            // Add more assertions as needed
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(invalidId)).ReturnsAsync((BookBorrowingRequest)null);

            // Act
            var result = await _service.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowExceptionForTooManyRequestsInMonth()
        {
            // Arrange
            var requestDTO = new BookBorrowingRequestDto
            {
                RequestorId = _userId,
                RequestDate = DateTime.UtcNow,
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetailDto>()
            };
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;
            var userRequestsThisMonth = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { RequestorId = _userId, RequestDate = new DateTime(currentYear, currentMonth, 1) },
                new BookBorrowingRequest { RequestorId = _userId, RequestDate = new DateTime(currentYear, currentMonth, 2) },
                new BookBorrowingRequest { RequestorId = _userId, RequestDate = new DateTime(currentYear, currentMonth, 3) }
            };
            _mockRepository.Setup(r => r.GetUserRequestsForMonthAsync(_userId, currentYear, currentMonth)).ReturnsAsync(userRequestsThisMonth);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(requestDTO));
        }

        [Fact]
        public async Task AddAsync_ShouldThrowExceptionForTooManyBooksInRequest()
        {
            // Arrange
            var requestDTO = new BookBorrowingRequestDto
            {
                RequestorId = _userId,
                RequestDate = DateTime.UtcNow,
                BookBorrowingRequestDetails = Enumerable.Range(1, 6).Select(_ => new BookBorrowingRequestDetailDto()).ToList()
            };
            _mockRepository.Setup(r => r.GetUserRequestsForMonthAsync(_userId, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<BookBorrowingRequest>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(requestDTO));
        }

        [Fact]
        public async Task AddAsync_WithValidRequest_ShouldReturnRequestDto()
        {
            // Arrange
            var requestDTO = new BookBorrowingRequestDto
            {
                RequestorId = _userId,
                RequestDate = DateTime.UtcNow,
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetailDto>
                {
                    new BookBorrowingRequestDetailDto { BookId = Guid.NewGuid() },
                    new BookBorrowingRequestDetailDto { BookId = Guid.NewGuid() }
                }
            };
            var createdRequest = new BookBorrowingRequest
            {
                Id = Guid.NewGuid(),
                RequestorId = _userId,
                RequestDate = DateTime.UtcNow,
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetail>
                {
                    new BookBorrowingRequestDetail { BookId = requestDTO.BookBorrowingRequestDetails[0].BookId },
                    new BookBorrowingRequestDetail { BookId = requestDTO.BookBorrowingRequestDetails[1].BookId }
                }
            };
            _mockRepository.Setup(r => r.GetUserRequestsForMonthAsync(_userId, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<BookBorrowingRequest>());
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<BookBorrowingRequest>())).ReturnsAsync(createdRequest);

            // Act
            var result = await _service.AddAsync(requestDTO);

            // Assert
            Assert.Equal(createdRequest.Id, result.Id);
            Assert.Equal(createdRequest.RequestorId, result.RequestorId);
            Assert.Equal(createdRequest.RequestDate, result.RequestDate);
            Assert.Equal(createdRequest.BookBorrowingRequestDetails.Count, result.BookBorrowingRequestDetails.Count);
            // Add more assertions as needed
        }

        [Fact]
        public async Task UpdateAsync_WithValidRequest_ShouldUpdateRequest()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var requestDTO = new BookBorrowingRequestDto { Id = requestId, RequestorId = _userId };

            // Act
            await _service.UpdateAsync(requestDTO);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<BookBorrowingRequest>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteRequest()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            // Act
            await _service.DeleteAsync(requestId);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(requestId), Times.Once);
        }

        [Fact]
        public async Task ApproveRequestAsync_WithValidRequest_ShouldApproveRequest()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var approverId = Guid.NewGuid();
            var request = new BookBorrowingRequest { Id = requestId, Status = RequestStatus.Waiting };
            _mockRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(request);

            // Act
            await _service.ApproveRequestAsync(requestId, approverId);

            // Assert
            Assert.Equal(RequestStatus.Approved, request.Status);
            Assert.Equal(approverId, request.ApproverId);
            _mockRepository.Verify(r => r.UpdateAsync(request), Times.Once);
        }

        [Fact]
        public async Task RejectRequestAsync_WithValidRequest_ShouldRejectRequest()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var approverId = Guid.NewGuid();
            var request = new BookBorrowingRequest { Id = requestId, Status = RequestStatus.Waiting };
            _mockRepository.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync(request);

            // Act
            await _service.RejectRequestAsync(requestId, approverId);

            // Assert
            Assert.Equal(RequestStatus.Rejected, request.Status);
            Assert.Equal(approverId, request.ApproverId);
            _mockRepository.Verify(r => r.UpdateAsync(request), Times.Once);
        }
    }
}