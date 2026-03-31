using InoxServer.Application.Features.Reviews.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.SharedKernel.Common;
using MediatR;

namespace InoxServer.Application.Features.Reviews.Queries.GetAllReviews
{
    public record GetAllReviewsQuery(
        Guid? ProductId = null,
        Guid? UserId = null,
        int Page = 1,
        int PageSize = 10) : IRequest<PagedResult<ReviewDto>>;
}
