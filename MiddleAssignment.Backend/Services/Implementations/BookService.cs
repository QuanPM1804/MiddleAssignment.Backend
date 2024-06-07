using AutoMapper;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Interfaces;

namespace MiddleAssignment.Backend.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<BookDTO> GetBookById(Guid id)
        {
            var book = await _bookRepository.GetBookById(id);
            return _mapper.Map<BookDTO>(book);
        }

        public async Task<BookDTO> CreateBook(BookDTO bookDTO)
        {
            var book = _mapper.Map<Book>(bookDTO);
            var createdBook = await _bookRepository.CreateBook(book);
            return _mapper.Map<BookDTO>(createdBook);
        }

        public async Task<BookDTO> UpdateBook(Guid id, BookDTO bookDTO)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
                return null;

            _mapper.Map(bookDTO, book);
            var updatedBook = await _bookRepository.UpdateBook(book);
            return _mapper.Map<BookDTO>(updatedBook);
        }

        public async Task DeleteBook(Guid id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (!book.IsAvailable)
            {
                throw new BadHttpRequestException("Cannot delete a book that has borrowed.");
            }
            await _bookRepository.DeleteBook(id);
        }
    }
}
