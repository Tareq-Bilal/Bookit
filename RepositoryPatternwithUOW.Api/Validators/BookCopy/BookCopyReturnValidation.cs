using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.BookCopy;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Constants;
using System.Linq;

namespace RepositoryPatternwithUOW.Api.Validators.BookCopy
{
    public class BookCopyReturnValidation : AbstractValidator<BookCopyReturnDTO>
    {
        private readonly IUnitOfWork _unitOfWork ;

        public BookCopyReturnValidation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ;

            RuleFor(x => x.Condition)
                .Must(_IsValidBookCopyCondition)
                .WithMessage("Book Copy Condition Should Be One Of : {\"Good\" , \"Damaged\"}");

            RuleFor(x => x.AdditionalCharges)
                .GreaterThan(0)
                .WithMessage("Additional Charges Should Be Greater Than Zero");

        }

        private bool _IsValidBookCopyCondition(string condition)
        {
            return BookReturnCondition.Conditions.Contains(condition);
        }


    }
}
