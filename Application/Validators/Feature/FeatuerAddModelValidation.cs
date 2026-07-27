using Application.DTOs.Feature;
using FluentValidation;

namespace Application.Validators.Feature
{
    public class FeatuerAddModelValidation : AbstractValidator<FeatureAddModel>
    {
        public FeatuerAddModelValidation()
        {
            RuleFor(x => x.FeatureName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.FeatureDescription)
                .MaximumLength(1000);
        }
    }
}