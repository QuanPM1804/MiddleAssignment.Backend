using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Backend.Services.Interfaces
{
    public interface IBookBorrowingRequestService
    {
        Task<BookBorrowingRequestDto> GetByIdAsync(Guid id);
        Task<IEnumerable<BookBorrowingRequestDto>> GetAllAsync();
        Task<BookBorrowingRequestDto> AddAsync(BookBorrowingRequestDto requestDTO);
        Task UpdateAsync(BookBorrowingRequestDto requestDTO);
        Task DeleteAsync(Guid id);
        Task ApproveRequestAsync(Guid id, Guid approverId);
        Task RejectRequestAsync(Guid id, Guid approverId);
    }
}
