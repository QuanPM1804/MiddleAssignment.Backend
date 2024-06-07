using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Services.Implementations;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _bookService = new BookService(_bookRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsListOfBookDTOs()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = Guid.NewGuid(), Title = "Book 1", Author = "Author 1" },
                new Book { Id = Guid.NewGuid(), Title = "Book 2", Author = "Author 2" }
            };
            var bookDTOs = new List<BookDTO>
            {
                new BookDTO { Id = books[0].Id, Title = books[0].Title, Author = books[0].Author },
                new BookDTO { Id = books[1].Id, Title = books[1].Title, Author = books[1].Author }
            };
            _bookRepositoryMock.Setup(repo => repo.GetAllBooks()).ReturnsAsync(books);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<BookDTO>>(books)).Returns(bookDTOs);

            // Act
            var result = await _bookService.GetAllBooks();

            // Assert
            Assert.Equal(bookDTOs, result);
        }

        [Fact]
        public async Task GetBookById_ReturnsBookDTO()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book { Id = bookId, Title = "Book 1", Author = "Author 1" };
            var bookDTO = new BookDTO { Id = book.Id, Title = book.Title, Author = book.Author };
            _bookRepositoryMock.Setup(repo => repo.GetBookById(bookId)).ReturnsAsync(book);
            _mapperMock.Setup(mapper => mapper.Map<BookDTO>(book)).Returns(bookDTO);

            // Act
            var result = await _bookService.GetBookById(bookId);

            // Assert
            Assert.Equal(bookDTO, result);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedBookDTO()
        {
            // Arrange
            var bookDTO = new BookDTO { Title = "New Book", Author = "New Author" };
            var book = new Book { Title = bookDTO.Title, Author = bookDTO.Author };
            var createdBook = new Book { Id = Guid.NewGuid(), Title = bookDTO.Title, Author = bookDTO.Author };
            var createdBookDTO = new BookDTO { Id = createdBook.Id, Title = createdBook.Title, Author = createdBook.Author };
            _mapperMock.Setup(mapper => mapper.Map<Book>(bookDTO)).Returns(book);
            _bookRepositoryMock.Setup(repo => repo.CreateBook(book)).ReturnsAsync(createdBook);
            _mapperMock.Setup(mapper => mapper.Map<BookDTO>(createdBook)).Returns(createdBookDTO);

            // Act
            var result = await _bookService.CreateBook(bookDTO);

            // Assert
            Assert.Equal(createdBookDTO, result);
        }

        [Fact]
        public async Task UpdateBook_ReturnsUpdatedBookDTO()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookDTO = new BookDTO { Id = bookId, Title = "Updated Book", Author = "Updated Author" };
            var book = new Book { Id = bookId, Title = "Book 1", Author = "Author 1" };
            var updatedBook = new Book { Id = bookId, Title = bookDTO.Title, Author = bookDTO.Author };
            var updatedBookDTO = new BookDTO { Id = updatedBook.Id, Title = updatedBook.Title, Author = updatedBook.Author };
            _bookRepositoryMock.Setup(repo => repo.GetBookById(bookId)).ReturnsAsync(book);
            _bookRepositoryMock.Setup(repo => repo.UpdateBook(updatedBook)).ReturnsAsync(updatedBook);
            _mapperMock.Setup(mapper => mapper.Map<BookDTO>(updatedBook)).Returns(updatedBookDTO);

            // Act
            var result = await _bookService.UpdateBook(bookId, bookDTO);

            // Assert
            Assert.Equal(updatedBookDTO, result);
        }

        [Fact]
        public async Task UpdateBook_WithNonExistingBook_ReturnsNull()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookDTO = new BookDTO { Id = bookId, Title = "Updated Book", Author = "Updated Author" };
            _bookRepositoryMock.Setup(repo => repo.GetBookById(bookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _bookService.UpdateBook(bookId, bookDTO);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteBook_CallsDeleteBookOnRepository()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookRepositoryMock.Setup(repo => repo.DeleteBook(bookId)).Returns(Task.CompletedTask);

            // Act
            await _bookService.DeleteBook(bookId);

            // Assert
            _bookRepositoryMock.Verify(repo => repo.DeleteBook(bookId), Times.Once);
        }
    }
}