using FluentValidation;
using InoxServer.Domain.Errors;

namespace InoxServer.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.Product.NotFound)
            .WithMessage("ID sản phẩm không được để trống.");

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

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Giá sản phẩm phải lớn hơn 0.");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0)
            .When(x => x.SalePrice.HasValue)
            .WithMessage("Giá khuyến mãi phải lớn hơn 0.");

        RuleFor(x => x.StockQty)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Số lượng tồn kho không được âm.");
    }
}