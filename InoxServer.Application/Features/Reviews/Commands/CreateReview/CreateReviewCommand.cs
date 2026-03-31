using MediatR;

namespace InoxServer.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid? OrderId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
    }
}
