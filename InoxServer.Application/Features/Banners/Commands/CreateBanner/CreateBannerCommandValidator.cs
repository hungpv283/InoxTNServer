using FluentValidation;

namespace InoxServer.Application.Features.Banners.Commands.CreateBanner;

public class CreateBannerCommandValidator : AbstractValidator<CreateBannerCommand>
{
    public CreateBannerCommandValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .WithMessage("Hình ảnh không được để trống.")
            .MaximumLength(500)
            .WithMessage("URL hình ảnh không được vượt quá 500 ký tự.");

        RuleFor(x => x.LinkUrl)
            .MaximumLength(500)
            .WithMessage("URL liên kết không được vượt quá 500 ký tự.")
            .Must(BeValidUrl)
            .When(x => !string.IsNullOrWhiteSpace(x.LinkUrl))
            .WithMessage("URL liên kết không hợp lệ.");

        RuleFor(x => x.Title)
            .MaximumLength(200)
            .WithMessage("Tiêu đề không được vượt quá 200 ký tự.");
    }

    private static bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
