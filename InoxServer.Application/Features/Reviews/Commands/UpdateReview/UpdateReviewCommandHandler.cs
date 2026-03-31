using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.Id, cancellationToken);
            if (review == null)
                throw new DomainException(ReviewErrors.NotFound);

            // Chỉ chủ sở hữu mới được sửa
            if (review.UserId != request.UserId)
                throw new DomainException(ReviewErrors.Unauthorized);

            review.Rating = request.Rating;
            review.Comment = request.Comment;

            _reviewRepository.Update(review);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
