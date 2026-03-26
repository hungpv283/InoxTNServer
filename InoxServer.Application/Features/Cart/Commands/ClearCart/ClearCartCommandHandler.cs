using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.ClearCart;

public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, bool>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClearCartCommandHandler(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        // Idempotent: nếu chưa có cart thì coi như đã clear xong
        if (cart is null)
            return true;

        if (!cart.CartItems.Any())
            return true;

        foreach (var item in cart.CartItems.ToList())
            _cartItemRepository.Delete(item);

        cart.UpdatedAt = DateTime.UtcNow;
        _cartRepository.Update(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

