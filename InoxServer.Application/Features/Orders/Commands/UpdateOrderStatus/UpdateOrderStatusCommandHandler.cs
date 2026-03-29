using InoxServer.Domain.Entities;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException(OrderErrors.NotFound);

        if (order.Status == request.NewStatus)
            return true;

        var from = order.Status;
        var utcNow = DateTime.UtcNow;

        order.Status = request.NewStatus;
        order.UpdatedAt = utcNow;

        var log = new OrderStatusLog
        {
            OrderId = order.Id,
            AdminId = request.AdminUserId,
            StatusFrom = from,
            StatusTo = request.NewStatus,
            Note = request.Note,
            ChangedAt = utcNow
        };
        order.OrderStatusLogs.Add(log);

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
