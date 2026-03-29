using FluentValidation;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Orders.Commands.UpdateOrderStatus;

public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.NewStatus)
            .NotEqual(OrderStatus.Unknown)
            .WithErrorCode(DomainErrorCodes.Order.InvalidStatus)
            .WithMessage("Trạng thái không hợp lệ.");

        RuleFor(x => x.Note)
            .MaximumLength(1000)
            .WithErrorCode(DomainErrorCodes.General.InvalidOperation)
            .WithMessage("Ghi chú không được vượt quá 1000 ký tự.");
    }
}
