using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.RemoveCartItem;

public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, bool>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCartItemCommandHandler(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart is null)
            throw new DomainException(CartErrors.NotFound);

        var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (cartItem is null)
            throw new DomainException(CartErrors.ItemNotFound);

        _cartItemRepository.Delete(cartItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

