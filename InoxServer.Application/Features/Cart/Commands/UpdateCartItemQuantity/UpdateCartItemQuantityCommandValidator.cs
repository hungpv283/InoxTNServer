using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Cart.Commands.UpdateCartItemQuantity;

public sealed class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
{
    public UpdateCartItemQuantityCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan((short)0)
            .WithErrorCode(DomainErrorCodes.Cart.QuantityInvalid)
            .WithMessage("Số lượng phải lớn hơn 0.");
    }
}

