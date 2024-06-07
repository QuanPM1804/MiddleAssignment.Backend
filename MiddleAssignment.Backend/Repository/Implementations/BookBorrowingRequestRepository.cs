using Microsoft.EntityFrameworkCore;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.Repository.Interfaces;

namespace MiddleAssignment.Backend.Repository.Implementations
{
    public class BookBorrowingRequestRepository : IBookBorrowingRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public BookBorrowingRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookBorrowingRequest> GetByIdAsync(Guid id)
        {
            return await _context.BookBorrowingRequests.Include(r => r.BookBorrowingRequestDetails)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetAllAsync()
        {
            return await _context.BookBorrowingRequests.Include(r => r.BookBorrowingRequestDetails)
                .ToListAsync();
        }

        public async Task<BookBorrowingRequest> AddAsync(BookBorrowingRequest request)
        {
            await _context.BookBorrowingRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task UpdateAsync(BookBorrowingRequest request)
        {
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var request = await _context.BookBorrowingRequests.FindAsync(id);
            if (request != null)
            {
                _context.BookBorrowingRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetUserRequestsForMonthAsync(Guid userId, int year, int month)
        {
            return await _context.BookBorrowingRequests
                .Where(r => r.RequestorId == userId && r.RequestDate.Year == year && r.RequestDate.Month == month)
                .ToListAsync();
        }
    }
}
