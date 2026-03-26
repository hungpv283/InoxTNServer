using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.UpdateCartItemQuantity;

public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, bool>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCartItemQuantityCommandHandler(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart is null)
            throw new DomainException(CartErrors.NotFound);

        var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (cartItem is null)
            throw new DomainException(CartErrors.ItemNotFound);

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new DomainException(CartErrors.ProductNotFound);

        if (!product.IsActive)
            throw new DomainException(CartErrors.ProductInactive);

        if (request.Quantity > product.StockQty)
            throw new DomainException(CartErrors.ExceedStock);

        var unitPrice = product.SalePrice ?? product.Price;

        cartItem.Quantity = request.Quantity;
        cartItem.UnitPrice = unitPrice;

        _cartItemRepository.Update(cartItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

