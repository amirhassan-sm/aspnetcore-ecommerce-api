using Application.DTOs.ProductFeatures;
using FluentValidation;

namespace Application.Validators.ProductFeature
{
    public class ProductFeatureValidators : AbstractValidator<SetProductFeatureModel>
    {
        public ProductFeatureValidators()
        {
            RuleFor(x => x.FeatureId)
                .GreaterThan(0);

            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.FeaureValue)
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}