using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Wishlist;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Validators.Wishlist
{
    public class WishlistAddDTOValidator : AbstractValidator<WishlistAddDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public WishlistAddDTOValidator(IUnitOfWork unitOfWork) {

            _unitOfWork = unitOfWork;

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
