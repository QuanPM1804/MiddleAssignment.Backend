using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.Services.Interfaces;
using System.Security.Claims;

namespace MiddleAssignment.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookBorrowingRequestController : ControllerBase
    {
        private readonly IBookBorrowingRequestService _requestService;

        public BookBorrowingRequestController(IBookBorrowingRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser, User")]
        public async Task<ActionResult<IEnumerable<BookBorrowingRequestDto>>> GetAllRequests()
        {
            var requests = await _requestService.GetAllAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperUser, User")]
        public async Task<ActionResult<BookBorrowingRequestDto>> GetRequestById(Guid id)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<BookBorrowingRequestDto>> CreateRequest(BookBorrowingRequestDto requestDTO)
        {
            try
            {
                var createdRequest = await _requestService.AddAsync(requestDTO);
                return CreatedAtAction(nameof(GetRequestById), new { id = createdRequest.Id }, createdRequest);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateRequest(Guid id, BookBorrowingRequestDto requestDTO)
        {
            if (id != requestDTO.Id)
            {
                return BadRequest();
            }

            await _requestService.UpdateAsync(requestDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            await _requestService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> ApproveRequest(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _requestService.ApproveRequestAsync(id, Guid.Parse(userId));
            return NoContent();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> RejectRequest(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _requestService.RejectRequestAsync(id, Guid.Parse(userId));
            return NoContent();
        }
    }
}
