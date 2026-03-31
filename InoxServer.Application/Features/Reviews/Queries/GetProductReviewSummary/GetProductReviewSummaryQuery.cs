using InoxServer.Application.Features.Reviews.DTOs;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Reviews.Queries.GetProductReviewSummary
{
    public record GetProductReviewSummaryQuery(Guid ProductId) : IRequest<ReviewSummaryDto>;
}
