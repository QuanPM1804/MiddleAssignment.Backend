using System.ComponentModel.DataAnnotations;

namespace MiddleAssignment.Backend.Models
{
    public class BookBorrowingRequest
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid RequestorId { get; set; }
        [Required]
        public User Requestor { get; set; }
        [Required]
        public DateTime RequestDate { get; set; }
        [Required]
        public Guid? ApproverId { get; set; }
        [Required]
        public User Approver { get; set; }
        [Required]
        public RequestStatus Status { get; set; }
        public List<BookBorrowingRequestDetail> BookBorrowingRequestDetails { get; set; }
    }
    public enum RequestStatus
    {
        Waiting,
        Approved,
        Rejected
    }
}
