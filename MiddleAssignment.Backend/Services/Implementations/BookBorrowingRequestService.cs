using AutoMapper;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Interfaces;

namespace MiddleAssignment.Backend.Services.Implementations
{
    public class BookBorrowingRequestService : IBookBorrowingRequestService
    {
        private readonly IBookBorrowingRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public BookBorrowingRequestService(IBookBorrowingRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }

        public async Task<BookBorrowingRequestDto> GetByIdAsync(Guid id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            return _mapper.Map<BookBorrowingRequestDto>(request);
        }

        public async Task<IEnumerable<BookBorrowingRequestDto>> GetAllAsync()
        {
            var requests = await _requestRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookBorrowingRequestDto>>(requests);
        }

        public async Task<BookBorrowingRequestDto> AddAsync(BookBorrowingRequestDto requestDTO)
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;
            var userRequestsThisMonth = await _requestRepository.GetUserRequestsForMonthAsync(requestDTO.RequestorId, currentYear, currentMonth);

            if (userRequestsThisMonth.Count() >= 3)
            {
                throw new InvalidOperationException("User has reached the limit of 3 borrowing requests per month.");
            }

            // Check if the request contains more than 5 books
            if (requestDTO.BookBorrowingRequestDetails.Count > 5)
            {
                throw new InvalidOperationException("A borrowing request cannot contain more than 5 books.");
            }

            var request = _mapper.Map<BookBorrowingRequest>(requestDTO);
            var addedRequest = await _requestRepository.AddAsync(request);
            return _mapper.Map<BookBorrowingRequestDto>(addedRequest);
        }

        public async Task UpdateAsync(BookBorrowingRequestDto requestDTO)
        {
            var request = _mapper.Map<BookBorrowingRequest>(requestDTO);
            await _requestRepository.UpdateAsync(request);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _requestRepository.DeleteAsync(id);
        }

        public async Task ApproveRequestAsync(Guid id, Guid approverId)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request != null)
            {
                request.Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), "Approved");
                request.ApproverId = approverId;
                await _requestRepository.UpdateAsync(request);
            }
        }

        public async Task RejectRequestAsync(Guid id, Guid approverId)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request != null)
            {
                request.Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), "Rejected");
                request.ApproverId = approverId;
                await _requestRepository.UpdateAsync(request);
            }
        }
    }
}
