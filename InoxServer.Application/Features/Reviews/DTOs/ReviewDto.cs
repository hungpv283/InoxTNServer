namespace InoxServer.Application.Features.Reviews.DTOs
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid? OrderId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReviewSummaryDto
    {
        public Guid ProductId { get; set; }
        public decimal AvgRating { get; set; }
        public int TotalReviews { get; set; }
        public int FiveStar { get; set; }
        public int FourStar { get; set; }
        public int ThreeStar { get; set; }
        public int TwoStar { get; set; }
        public int OneStar { get; set; }
    }
}
