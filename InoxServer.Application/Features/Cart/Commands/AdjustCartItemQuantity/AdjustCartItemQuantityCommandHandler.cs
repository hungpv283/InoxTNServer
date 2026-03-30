using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.AdjustCartItemQuantity;

public class AdjustCartItemQuantityCommandHandler : IRequestHandler<AdjustCartItemQuantityCommand, bool>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdjustCartItemQuantityCommandHandler(
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

    public async Task<bool> Handle(AdjustCartItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new DomainException(CartErrors.ProductNotFound);

        if (!product.IsActive)
            throw new DomainException(CartErrors.ProductInactive);

        var unitPrice = product.SalePrice ?? product.Price;
        var utcNow = DateTime.UtcNow;

        var cart = await _cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart is null)
        {
            cart = new InoxServer.Domain.Entities.Cart
            {
                UserId = request.UserId,
                CreatedAt = utcNow,
                UpdatedAt = utcNow
            };

            await _cartRepository.AddAsync(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var existing = cart.CartItems.FirstOrDefault(x => x.ProductId == request.ProductId);

        if (existing is null)
        {
            if (request.Delta < 0)
                throw new DomainException(CartErrors.ItemNotFound);

            if (request.Delta > product.StockQty)
                throw new DomainException(CartErrors.ExceedStock);

            var item = new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Delta,
                UnitPrice = unitPrice,
                AddedAt = utcNow
            };

            await _cartItemRepository.AddAsync(item, cancellationToken);
            cart.UpdatedAt = utcNow;
            _cartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        var newQty = (int)existing.Quantity + request.Delta;

        if (newQty <= 0)
        {
            _cartItemRepository.Delete(existing);
            cart.UpdatedAt = utcNow;
            _cartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        if (newQty > product.StockQty)
            throw new DomainException(CartErrors.ExceedStock);

        existing.Quantity = (short)newQty;
        existing.UnitPrice = unitPrice;

        cart.UpdatedAt = utcNow;

        _cartItemRepository.Update(existing);
        _cartRepository.Update(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

