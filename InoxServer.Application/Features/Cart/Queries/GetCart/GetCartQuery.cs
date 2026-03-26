using InoxServer.Application.Features.Cart.DTOs;
using MediatR;

namespace InoxServer.Application.Features.Cart.Queries.GetCart;

public class GetCartQuery : IRequest<CartDto>
{
    public Guid UserId { get; set; }
}

