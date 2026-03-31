using FluentValidation;

namespace InoxServer.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("ReviewId không được để trống.");

            RuleFor(x => x.Rating)
                .InclusiveBetween((byte)1, (byte)5)
                .WithMessage("Điểm đánh giá phải từ 1 đến 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(2000)
                .WithMessage("Bình luận không được vượt quá 2000 ký tự.");
        }
    }
}
