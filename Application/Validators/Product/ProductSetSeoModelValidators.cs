using Application.DTOs.Product;
using FluentValidation;

namespace Application.Validators.Product
{
    public class ProductSetSeoModelValidators : AbstractValidator<ProductSeoModel>
    {
        public ProductSetSeoModelValidators()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.ExtraData)
                .MaximumLength(5000);

            RuleFor(x => x.BodyExtraData)
                .MaximumLength(5000);

            RuleFor(x => x.Slug)
                .MaximumLength(200)
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .When(x => !string.IsNullOrWhiteSpace(x.Slug))
                .WithMessage("Slug must contain only lowercase letters, numbers, and hyphens.");

            RuleFor(x => x.PageTitle)
                .MaximumLength(200);

            RuleFor(x => x.MetaTag)
                .MaximumLength(500);

            RuleFor(x => x.MetaDescription)
                .MaximumLength(1000);
        }
    }
}