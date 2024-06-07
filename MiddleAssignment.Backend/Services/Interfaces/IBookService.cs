using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Backend.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetAllBooks();
        Task<BookDTO> GetBookById(Guid id);
        Task<BookDTO> CreateBook(BookDTO bookDTO);
        Task<BookDTO> UpdateBook(Guid id, BookDTO bookDTO);
        Task DeleteBook(Guid id);
    }
}
