using InoxServer.Application.Features.Reviews.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.SharedKernel.Common;
using MediatR;

namespace InoxServer.Application.Features.Reviews.Queries.GetReviewsByUser
{
    public class GetReviewsByUserQueryHandler : IRequestHandler<GetReviewsByUserQuery, PagedResult<ReviewDto>>
    {
        private readonly IReviewRepository _reviewRepository;

        public GetReviewsByUserQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<PagedResult<ReviewDto>> Handle(GetReviewsByUserQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _reviewRepository.GetPagedByUserAsync(
                request.UserId,
                request.Page,
                request.PageSize,
                cancellationToken);

            var items = pagedResult.Items.Select(x => new ReviewDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product?.Name,
                UserId = x.UserId,
                UserName = x.User?.Name,
                OrderId = x.OrderId,
                Rating = x.Rating,
                Comment = x.Comment,
                IsApproved = x.IsApproved,
                CreatedAt = x.CreatedAt
            }).ToList();

            return new PagedResult<ReviewDto>
            {
                Items = items,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }
    }
}
