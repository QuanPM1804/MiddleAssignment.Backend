using MiddleAssignment.Backend.Models;

namespace MiddleAssignment.Backend.Repository.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(Guid id);
        Task<Book> CreateBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task DeleteBook(Guid id);
    }
}
