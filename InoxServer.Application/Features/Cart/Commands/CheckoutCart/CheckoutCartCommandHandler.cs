using InoxServer.Application.Features.Cart.DTOs;
using InoxServer.Domain.Configuration;
using InoxServer.Domain.Entities;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace InoxServer.Application.Features.Cart.Commands.CheckoutCart;

public class CheckoutCartCommandHandler : IRequestHandler<CheckoutCartCommand, CheckoutCartResponseDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPayOsPaymentClient _payOsPaymentClient;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppOptions _appOptions;

    public CheckoutCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository,
        IPayOsPaymentClient payOsPaymentClient,
        IUnitOfWork unitOfWork,
        IOptions<AppOptions> appOptions)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
        _payOsPaymentClient = payOsPaymentClient;
        _unitOfWork = unitOfWork;
        _appOptions = appOptions.Value;
    }

    public async Task<CheckoutCartResponseDto> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        var built = await _unitOfWork.ExecuteInTransactionAsync(
            async () => await BuildOrderAndPaymentAsync(request, cancellationToken),
            cancellationToken);

        if (request.PaymentMethod != PaymentMethod.PayOS)
        {
            return new CheckoutCartResponseDto
            {
                OrderId = built.Order.Id,
                OrderNumber = built.Order.OrderNumber,
                TotalAmount = built.Order.TotalAmount,
                PaymentMethod = request.PaymentMethod,
                PaymentId = built.Payment.Id,
                PayOsCheckoutUrl = null,
                PayOsQrCode = null
            };
        }

        var amountVnd = (long)Math.Round(built.Order.TotalAmount, MidpointRounding.AwayFromZero);
        var desc = built.Order.OrderNumber.Length <= 9 ? built.Order.OrderNumber : built.Order.OrderNumber[..9];

        var link = await _payOsPaymentClient.CreatePaymentLinkAsync(
            built.Payment.PayosOrderCode!.Value,
            amountVnd,
            desc,
            built.ReturnUrl,
            built.CancelUrl,
            cancellationToken);

        await _unitOfWork.ExecuteInTransactionAsync(
            async () =>
            {
                var payment = await _paymentRepository.GetByIdAsync(built.Payment.Id, cancellationToken);
                if (payment is null)
                    throw new DomainException(PaymentErrors.NotFound);

                payment.PayosCheckoutUrl = link.CheckoutUrl;
                payment.PayosQrCode = link.QrCode;
                payment.PayosPaymentLinkId = link.PaymentLinkId;
                _paymentRepository.Update(payment);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            },
            cancellationToken);

        return new CheckoutCartResponseDto
        {
            OrderId = built.Order.Id,
            OrderNumber = built.Order.OrderNumber,
            TotalAmount = built.Order.TotalAmount,
            PaymentMethod = request.PaymentMethod,
            PaymentId = built.Payment.Id,
            PayOsCheckoutUrl = link.CheckoutUrl,
            PayOsQrCode = link.QrCode
        };
    }

    private async Task<CheckoutBuiltState> BuildOrderAndPaymentAsync(
        CheckoutCartCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (cart is null)
            throw new DomainException(CartErrors.NotFound);

        if (!cart.CartItems.Any())
            throw new DomainException(CartErrors.CheckoutEmpty);

        if (request.PaymentMethod != PaymentMethod.Cod && request.PaymentMethod != PaymentMethod.PayOS)
            throw new DomainException(
                Error.BadRequest(DomainErrorCodes.General.InvalidOperation, "Chỉ hỗ trợ thanh toán COD hoặc PayOS."));

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

        Payment payment;

        if (request.PaymentMethod == PaymentMethod.Cod)
        {
            payment = new Payment
            {
                OrderId = order.Id,
                Method = PaymentMethod.Cod,
                Status = PaymentStatus.Pending,
                Amount = order.TotalAmount,
                CreatedAt = utcNow
            };
            await _paymentRepository.AddAsync(payment, cancellationToken);
        }
        else
        {
            var payOsCode = await GenerateUniquePayOsOrderCode(cancellationToken);

            payment = new Payment
            {
                OrderId = order.Id,
                Method = PaymentMethod.PayOS,
                Status = PaymentStatus.Pending,
                Amount = order.TotalAmount,
                CreatedAt = utcNow,
                PayosOrderCode = payOsCode
            };
            await _paymentRepository.AddAsync(payment, cancellationToken);
        }

        _cartRepository.Delete(cart);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var baseFront = _appOptions.FrontendUrl.TrimEnd('/');
        var returnUrl = string.IsNullOrWhiteSpace(request.PayOsReturnUrl)
            ? $"{baseFront}/payment/return?orderId={order.Id}"
            : request.PayOsReturnUrl!;
        var cancelUrl = string.IsNullOrWhiteSpace(request.PayOsCancelUrl)
            ? $"{baseFront}/payment/cancel?orderId={order.Id}"
            : request.PayOsCancelUrl!;

        return new CheckoutBuiltState(order, payment, returnUrl, cancelUrl);
    }

    private sealed record CheckoutBuiltState(Order Order, Payment Payment, string ReturnUrl, string CancelUrl);

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

        return $"INX{DateTime.UtcNow:yyyyMMddHHmmss}{Guid.NewGuid():N}";
    }

    private async Task<long> GenerateUniquePayOsOrderCode(CancellationToken cancellationToken)
    {
        for (var i = 0; i < 15; i++)
        {
            var code = Random.Shared.NextInt64(100_000_000, int.MaxValue);
            if (!await _paymentRepository.ExistsByPayosOrderCodeAsync(code, cancellationToken))
                return code;
        }

        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
