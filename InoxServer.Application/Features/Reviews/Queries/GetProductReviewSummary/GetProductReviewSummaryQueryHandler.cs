using InoxServer.Application.Features.Reviews.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Reviews.Queries.GetProductReviewSummary
{
    public class GetProductReviewSummaryQueryHandler : IRequestHandler<GetProductReviewSummaryQuery, ReviewSummaryDto>
    {
        private readonly IReviewRepository _reviewRepository;

        public GetProductReviewSummaryQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewSummaryDto> Handle(GetProductReviewSummaryQuery request, CancellationToken cancellationToken)
        {
            var summary = await _reviewRepository.GetSummaryByProductAsync(request.ProductId, cancellationToken);

            return new ReviewSummaryDto
            {
                ProductId = summary.ProductId,
                AvgRating = summary.AvgRating,
                TotalReviews = summary.TotalReviews,
                FiveStar = summary.FiveStar,
                FourStar = summary.FourStar,
                ThreeStar = summary.ThreeStar,
                TwoStar = summary.TwoStar,
                OneStar = summary.OneStar
            };
        }
    }
}
