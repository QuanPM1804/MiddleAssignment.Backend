using MiddleAssignment.Backend.Models;

namespace MiddleAssignment.Backend.DTOs
{
    public class BookBorrowingRequestDto
    {
        public Guid Id { get; set; }
        public Guid RequestorId { get; set; }
        public DateTime RequestDate { get; set; }
        public Guid? ApproverId { get; set; }
        public RequestStatus Status { get; set; }
        public List<BookBorrowingRequestDetailDto> BookBorrowingRequestDetails { get; set; }
    }

}
