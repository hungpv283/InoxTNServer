using FluentValidation;

namespace InoxServer.Application.Features.Wishlists.Commands.AddToWishlist;

public class AddToWishlistCommandValidator : AbstractValidator<AddToWishlistCommand>
{
    public AddToWishlistCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId không được để trống.");
    }
}