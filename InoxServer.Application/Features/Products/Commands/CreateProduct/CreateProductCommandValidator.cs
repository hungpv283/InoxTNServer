using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Product.CategoryNotFound)
            .WithMessage("Danh mục sản phẩm không được để trống.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Product.NameAlreadyExists)
            .WithMessage("Tên sản phẩm không được để trống.")
            .MaximumLength(200)
            .WithMessage("Tên sản phẩm không được vượt quá 200 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Product.SlugAlreadyExists)
            .WithMessage("Slug không được để trống.")
            .MaximumLength(200)
            .WithMessage("Slug không được vượt quá 200 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ thường, số và dấu gạch ngang.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithErrorCode(DomainErrorCodes.Product.InsufficientStock)
            .WithMessage("Giá sản phẩm phải lớn hơn 0.");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0)
            .When(x => x.SalePrice.HasValue)
            .WithMessage("Giá khuyến mãi phải lớn hơn 0.");

        RuleFor(x => x.StockQty)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Số lượng tồn kho không được âm.");

        RuleFor(x => x.Sku)
            .NotEmpty()
            .WithMessage("SKU không được để trống.")
            .MaximumLength(100)
            .WithMessage("SKU không được vượt quá 100 ký tự.");
    }
}