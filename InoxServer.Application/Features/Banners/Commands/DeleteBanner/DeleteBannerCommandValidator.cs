using FluentValidation;

namespace InoxServer.Application.Features.Banners.Commands.DeleteBanner;

public class DeleteBannerCommandValidator : AbstractValidator<DeleteBannerCommand>
{
    public DeleteBannerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID không được để trống.");
    }
}
