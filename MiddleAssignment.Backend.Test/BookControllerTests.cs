using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MiddleAssignment.Server.Controllers;
using MiddleAssignment.Backend.Services.Interfaces;
using MiddleAssignment.Backend.DTOs;

namespace MiddleAssignment.Tests.Controllers
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _controller = new BookController(_bookServiceMock.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult()
        {
            // Arrange
            var books = new List<BookDTO>
            {
                new BookDTO { Id = Guid.NewGuid(), Title = "Book 1", Author = "Author 1" },
                new BookDTO { Id = Guid.NewGuid(), Title = "Book 2", Author = "Author 2" }
            };
            _bookServiceMock.Setup(service => service.GetAllBooks()).ReturnsAsync(books);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(books, okResult.Value);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResult()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new BookDTO { Id = bookId, Title = "Book 1", Author = "Author 1" };
            _bookServiceMock.Setup(service => service.GetBookById(bookId)).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(book, okResult.Value);
        }

        [Fact]
        public async Task GetBookById_ReturnsNotFoundResult()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookServiceMock.Setup(service => service.GetBookById(bookId)).ReturnsAsync((BookDTO)null);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var bookDTO = new BookDTO { Title = "New Book", Author = "New Author" };
            var createdBook = new BookDTO { Id = Guid.NewGuid(), Title = "New Book", Author = "New Author" };
            _bookServiceMock.Setup(service => service.CreateBook(bookDTO)).ReturnsAsync(createdBook);

            // Act
            var result = await _controller.CreateBook(bookDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.Equal(createdBook, createdAtActionResult.Value);
            Assert.Equal("GetBookById", createdAtActionResult.ActionName);
            Assert.Equal(createdBook.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public async Task UpdateBook_ReturnsOkResult()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookDTO = new BookDTO { Id = bookId, Title = "Updated Book", Author = "Updated Author" };
            _bookServiceMock.Setup(service => service.UpdateBook(bookId, bookDTO)).ReturnsAsync(bookDTO);

            // Act
            var result = await _controller.UpdateBook(bookId, bookDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(bookDTO, okResult.Value);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNotFoundResult()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookDTO = new BookDTO { Id = bookId, Title = "Updated Book", Author = "Updated Author" };
            _bookServiceMock.Setup(service => service.UpdateBook(bookId, bookDTO)).ReturnsAsync((BookDTO)null);

            // Act
            var result = await _controller.UpdateBook(bookId, bookDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContentResult()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _bookServiceMock.Setup(service => service.DeleteBook(bookId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBook(bookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}