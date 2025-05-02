using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Category;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Validators.Category
{
    public class CategoryUpdateDTOValidator : AbstractValidator<CategoryUpdateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryUpdateDTOValidator(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.")
                .Must(_IsCategoryNameTaken).WithMessage("Category Name Is Already Taken !");

        }

        private bool _IsCategoryNameTaken(string categoryName)
        {
            var isTaken = _unitOfWork.Categories.FindAsync(c => c.Name == categoryName);

            return (isTaken == null ? false : true);

        }


    }
}
