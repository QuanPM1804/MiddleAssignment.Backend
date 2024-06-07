namespace MiddleAssignment.Backend.DTOs
{
    public class BookBorrowingRequestDetailDto
    {
        public Guid Id { get; set; }
        public Guid BookBorrowingRequestId { get; set; }
        public Guid BookId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
