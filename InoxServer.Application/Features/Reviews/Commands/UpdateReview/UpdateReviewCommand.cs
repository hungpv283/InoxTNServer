using MediatR;

namespace InoxServer.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
    }
}
