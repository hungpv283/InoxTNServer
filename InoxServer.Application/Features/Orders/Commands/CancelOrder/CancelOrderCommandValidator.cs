using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithErrorCode(DomainErrorCodes.General.InvalidOperation)
            .WithMessage("Lý do hủy không được vượt quá 500 ký tự.");
    }
}
