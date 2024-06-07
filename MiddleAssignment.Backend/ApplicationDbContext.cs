using Microsoft.EntityFrameworkCore;
using MiddleAssignment.Backend.Models;


namespace MiddleAssignment.Backend
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookBorrowingRequest> BookBorrowingRequests { get; set; }
        public DbSet<BookBorrowingRequestDetail> BookBorrowingRequestDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookBorrowingRequest>()
                .HasOne(r => r.Requestor)
                .WithMany(u => u.BookBorrowingRequests)
                .HasForeignKey(r => r.RequestorId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<BookBorrowingRequest>()
                .HasOne(r => r.Approver)
                .WithMany(u => u.BookBorrowingRequestsApproved)
                .HasForeignKey(r => r.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookBorrowingRequestDetail>()
                .HasOne(d => d.BookBorrowingRequest)
                .WithMany(r => r.BookBorrowingRequestDetails)
                .HasForeignKey(d => d.BookBorrowingRequestId);

            modelBuilder.Entity<BookBorrowingRequestDetail>()
                .HasOne(d => d.Book)
                .WithMany(b => b.BookBorrowingRequestDetails)
                .HasForeignKey(d => d.BookId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<BookBorrowingRequestDetail>()
                .HasKey(br => new { br.BookBorrowingRequestId, br.BookId });

        }

    }
}
