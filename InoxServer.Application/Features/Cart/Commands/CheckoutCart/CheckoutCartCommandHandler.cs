using InoxServer.Application.Features.Cart.DTOs;
using InoxServer.Domain.Entities;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Cart.Commands.CheckoutCart;

public class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand, CheckoutCartResponseDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CheckoutCartResponseDto> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart is null)
            throw new DomainException(CartErrors.NotFound);

        if (!cart.CartItems.Any())
            throw new DomainException(CartErrors.CheckoutEmpty);

        var utcNow = DateTime.UtcNow;

        decimal subtotal = 0;
        var order = new Order
        {
            UserId = request.UserId,
            ShippingFee = request.ShippingFee,
            DiscountAmount = 0,
            Status = OrderStatus.Pending,
            ShippingName = request.ShippingName,
            ShippingPhone = request.ShippingPhone,
            ShippingAddress = request.ShippingAddress,
            ShippingProvince = request.ShippingProvince,
            Note = request.Note,
            CreatedAt = utcNow,
            UpdatedAt = utcNow,
        };

        // Validate stock + build order items (and update product stock)
        foreach (var cartItem in cart.CartItems)
        {
            var product = cartItem.Product ?? await _productRepository.GetByIdAsync(cartItem.ProductId, cancellationToken);
            if (product is null)
                throw new DomainException(CartErrors.ProductNotFound);

            if (!product.IsActive)
                throw new DomainException(CartErrors.ProductInactive);

            if (cartItem.Quantity > product.StockQty)
                throw new DomainException(CartErrors.ExceedStock);

            var itemSubtotal = cartItem.UnitPrice * cartItem.Quantity;
            subtotal += itemSubtotal;

            product.StockQty -= cartItem.Quantity;
            product.UpdatedAt = utcNow;
            _productRepository.Update(product);

            var orderItem = new OrderItem
            {
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                Sku = product.Sku,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                Subtotal = itemSubtotal,
                Order = order
            };

            order.OrderItems.Add(orderItem);
        }

        order.OrderNumber = await GenerateUniqueOrderNumber(cancellationToken);
        order.Subtotal = subtotal;
        order.TotalAmount = subtotal + request.ShippingFee;

        await _orderRepository.AddAsync(order, cancellationToken);

        // Clear cart after successful checkout
        _cartRepository.Delete(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CheckoutCartResponseDto
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            TotalAmount = order.TotalAmount
        };
    }

    private async Task<string> GenerateUniqueOrderNumber(CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            var guidPart = Guid.NewGuid().ToString("N").Substring(0, 8);
            var orderNumber = $"INX{DateTime.UtcNow:yyyyMMddHHmmss}{guidPart}";

            var exists = await _orderRepository.ExistsByOrderNumberAsync(orderNumber, cancellationToken);
            if (!exists)
                return orderNumber;
        }

        // Fallback (collision is extremely unlikely)
        return $"INX{DateTime.UtcNow:yyyyMMddHHmmss}{Guid.NewGuid():N}";
    }
}

