using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Cart.Commands.AdjustCartItemQuantity;

public sealed class AdjustCartItemQuantityCommandValidator : AbstractValidator<AdjustCartItemQuantityCommand>
{
    public AdjustCartItemQuantityCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Cart.ProductNotFound)
            .WithMessage("Sản phẩm không hợp lệ.");

        RuleFor(x => x.Delta)
            .NotEqual((short)0)
            .WithErrorCode(DomainErrorCodes.Cart.QuantityInvalid)
            .WithMessage("Delta không hợp lệ.");
    }
}

