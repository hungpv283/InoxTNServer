using InoxServer.Application.Features.Reviews.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Reviews.Queries.GetReviewById
{
    public record GetReviewByIdQuery(Guid Id) : IRequest<ReviewDto?>;
}
