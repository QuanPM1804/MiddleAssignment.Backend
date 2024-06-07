using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.Services.Interfaces;

namespace MiddleAssignment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser, User")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,SuperUser")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookById(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        [Authorize (Roles = "SuperUser")]
        public async Task<IActionResult> CreateBook(BookDTO bookDTO)
        {
            var createdBook = await _bookService.CreateBook(bookDTO);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> UpdateBook(Guid id, BookDTO bookDTO)
        {
            var updatedBook = await _bookService.UpdateBook(id, bookDTO);
            if (updatedBook == null)
                return NotFound();

            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            await _bookService.DeleteBook(id);
            return NoContent();
        }
    }
}
