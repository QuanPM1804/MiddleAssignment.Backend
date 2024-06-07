using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.Services.Interfaces;

namespace MiddleAssignment.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [Authorize(Roles = "User,SuperUser")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,SuperUser")]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var review = await _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpGet("book/{bookId}")]
        [Authorize(Roles = "User,SuperUser")]
        public async Task<IActionResult> GetReviewsByBookId(Guid bookId)
        {
            var reviews = await _reviewService.GetReviewsByBookId(bookId);
            return Ok(reviews);
        }

        [HttpPost]
        [Authorize(Roles = "User,SuperUser")]
        public async Task<IActionResult> CreateReview(ReviewDTO reviewDTO)
        {
            var createdReview = await _reviewService.CreateReview(reviewDTO);
            return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.Id }, createdReview);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User,SuperUser")]
        public async Task<IActionResult> UpdateReview(Guid id, ReviewDTO reviewDTO)
        {
            var updatedReview = await _reviewService.UpdateReview(id, reviewDTO);
            if (updatedReview == null)
                return NotFound();

            return Ok(updatedReview);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User,SuperUser")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            await _reviewService.DeleteReview(id);
            return NoContent();
        }
    }
}
