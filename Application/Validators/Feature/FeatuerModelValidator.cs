using Application.DTOs.Feature;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Feature
{
    public class FeatuerModelValidator:AbstractValidator<FeatureModel>
    {
        public FeatuerModelValidator()
        {

            RuleFor(x => x.FeatureName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.FeatureDescription).MaximumLength(300);
            RuleFor(x => x.FeatureId).GreaterThan(0);
            
        }

    }
}
