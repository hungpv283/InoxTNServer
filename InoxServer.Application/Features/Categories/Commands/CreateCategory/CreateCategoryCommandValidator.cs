using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Category.NameAlreadyExists)
            .WithMessage("Tên danh mục không được để trống.")
            .MaximumLength(200)
            .WithMessage("Tên danh mục không được vượt quá 200 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Category.SlugAlreadyExists)
            .WithMessage("Slug không được để trống.")
            .MaximumLength(200)
            .WithMessage("Slug không được vượt quá 200 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ thường, số và dấu gạch ngang.");
    }
}