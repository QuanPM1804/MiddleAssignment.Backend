using MiddleAssignment.Backend.Models;

namespace MiddleAssignment.Backend.Repository.Interfaces
{
    public interface IBookBorrowingRequestRepository
    {
        Task<BookBorrowingRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<BookBorrowingRequest>> GetAllAsync();
        Task<BookBorrowingRequest> AddAsync(BookBorrowingRequest request);
        Task UpdateAsync(BookBorrowingRequest request);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<BookBorrowingRequest>> GetUserRequestsForMonthAsync(Guid userId, int year, int month);
    }
}
