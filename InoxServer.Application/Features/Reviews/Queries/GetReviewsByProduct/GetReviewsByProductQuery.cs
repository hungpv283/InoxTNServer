using InoxServer.Application.Features.Reviews.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.SharedKernel.Common;
using MediatR;

namespace InoxServer.Application.Features.Reviews.Queries.GetReviewsByProduct
{
    public record GetReviewsByProductQuery(
        Guid ProductId,
        int Page = 1,
        int PageSize = 10) : IRequest<PagedResult<ReviewDto>>;
}
