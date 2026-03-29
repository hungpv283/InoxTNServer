using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        return _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
                throw new DomainException(OrderErrors.NotFound);

            if (order.UserId != request.UserId)
                throw new DomainException(OrderErrors.UnauthorizedAccess);

            if (order.Status == OrderStatus.Cancelled)
                throw new DomainException(OrderErrors.AlreadyCancelled);

            // Khách chỉ được hủy khi đơn còn chờ xử lý (chưa giao hàng)
            if (order.Status != OrderStatus.Pending)
                throw new DomainException(OrderErrors.CannotCancel);

            var utcNow = DateTime.UtcNow;

            foreach (var line in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(line.ProductId, cancellationToken);
                if (product is null)
                    continue;

                product.StockQty += line.Quantity;
                product.UpdatedAt = utcNow;
                _productRepository.Update(product);
            }

            order.Status = OrderStatus.Cancelled;
            order.CancelledReason = request.Reason;
            order.UpdatedAt = utcNow;
            _orderRepository.Update(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }, cancellationToken);
    }
}
