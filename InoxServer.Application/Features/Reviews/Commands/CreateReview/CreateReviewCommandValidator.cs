using FluentValidation;

namespace InoxServer.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId không được để trống.");

            RuleFor(x => x.Rating)
                .InclusiveBetween((byte)1, (byte)5)
                .WithMessage("Điểm đánh giá phải từ 1 đến 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(2000)
                .WithMessage("Bình luận không được vượt quá 2000 ký tự.");

            RuleFor(x => x.OrderId)
                .Must(orderId => orderId == null || orderId != Guid.Empty)
                .WithMessage("OrderId không hợp lệ.");
        }
    }
}
