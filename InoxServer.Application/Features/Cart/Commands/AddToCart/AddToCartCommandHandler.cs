using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.AddToCart;

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Guid>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddToCartCommandHandler(
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

    public async Task<Guid> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new DomainException(CartErrors.ProductNotFound);

        if (!product.IsActive)
            throw new DomainException(CartErrors.ProductInactive);

        if (request.Quantity > product.StockQty)
            throw new DomainException(CartErrors.ExceedStock);

        var unitPrice = product.SalePrice ?? product.Price;

        var cart = await _cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (cart is null)
        {
            cart = new InoxServer.Domain.Entities.Cart
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var cartItem = new CartItem
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = unitPrice,
                AddedAt = DateTime.UtcNow,
                Cart = cart
            };

            cart.CartItems.Add(cartItem);

            await _cartRepository.AddAsync(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return cartItem.Id;
        }

        var existing = cart.CartItems.FirstOrDefault(x => x.ProductId == request.ProductId);
        if (existing is not null)
        {
            var newQty = (int)existing.Quantity + request.Quantity;
            if (newQty > product.StockQty)
                throw new DomainException(CartErrors.ExceedStock);

            existing.Quantity = (short)newQty;
            existing.UnitPrice = unitPrice;

            cart.UpdatedAt = DateTime.UtcNow;

            _cartItemRepository.Update(existing);
            _cartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return existing.Id;
        }

        var newCartItem = new CartItem
        {
            CartId = cart.Id,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = unitPrice,
            AddedAt = DateTime.UtcNow,
            Cart = cart
        };

        cart.UpdatedAt = DateTime.UtcNow;
        _cartRepository.Update(cart);

        await _cartItemRepository.AddAsync(newCartItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newCartItem.Id;
    }
}

