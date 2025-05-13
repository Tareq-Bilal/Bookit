using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Review;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Validators.Review
{
    public class ReviewAddDTOValidator : AbstractValidator<ReviewAddDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewAddDTOValidator(IUnitOfWork unitOfWork) { 
        
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Comment)
                .MaximumLength(300).WithMessage("Maximum Length Of Comment Is 300 Characters");

            RuleFor(x => x.Rating)
                .GreaterThanOrEqualTo(0).WithMessage("Rating Should Be Grater Than 0 ")
                .LessThanOrEqualTo(5).WithMessage("Rating Should Be Less Than 5");

            RuleFor(x => x.UserId)
                .Must(_IsUserExist).WithMessage("Not Found User !");

            RuleFor(x => x.BookId)
                .Must(_IsBookExist).WithMessage("Not Found Book !");
        }

        private bool _IsUserExist(int userId)
        {
            return _unitOfWork.Users.GetById(userId) != null;
        }

        private bool _IsBookExist(int bookId)
        {
            return _unitOfWork.Books.GetById(bookId) != null;
        }
    }
}
