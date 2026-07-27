using Application.DTOs.CategoryFeatureModel;
using FluentValidation;

namespace Application.Validators.Category
{
    public class CategoryFeatureModelValidator : AbstractValidator<CategoryFeatureModel>
    {
        public CategoryFeatureModelValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.FeatureId)
                .GreaterThan(0);

            RuleFor(x => x.CategoryFeatureId)
                .GreaterThan(0)
                .When(x => x.CategoryFeatureId != 0);
        }
    }
}