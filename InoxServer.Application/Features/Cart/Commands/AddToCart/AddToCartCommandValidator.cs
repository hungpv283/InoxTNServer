using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Cart.Commands.AddToCart;

public sealed class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Cart.ProductNotFound)
            .WithMessage("Sản phẩm không hợp lệ.");

        RuleFor(x => x.Quantity)
            .GreaterThan((short)0)
            .WithErrorCode(DomainErrorCodes.Cart.QuantityInvalid)
            .WithMessage("Số lượng phải lớn hơn 0.");
    }
}

