using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Review;

namespace RepositoryPatternwithUOW.Api.Validators.Review
{
    public class ReviewUpdateDTOValidator : AbstractValidator<ReviewUpdateDTO>
    {
        public ReviewUpdateDTOValidator() {

            RuleFor(x => x.Comment)
                .MaximumLength(300).WithMessage("Maximum Length Of Comment Is 300 Characters");

            RuleFor(x => x.Rating)
                .GreaterThanOrEqualTo(0).WithMessage("Rating Should Be Grater Than 0 ")
                .LessThanOrEqualTo(5).WithMessage("Rating Should Be Less Than 5");

        }
    }
}
