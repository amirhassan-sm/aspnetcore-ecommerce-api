using Application.DTOs.Product;
using FluentValidation;

namespace Application.Validators.Product
{
    public class ProductUpdateModelValidators : AbstractValidator<ProductUpdateModel>
    {
        public ProductUpdateModelValidators()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ProductDescription)
                .MaximumLength(5000);

            RuleFor(x => x.BasePrice)
                .GreaterThan(0);

            RuleFor(x => x.Introduction)
                .MaximumLength(3000);

            RuleFor(x => x.ExtraData)
                .MaximumLength(5000);

            RuleFor(x => x.BodyExtraData)
                .MaximumLength(5000);

            RuleFor(x => x.Slug)
                .NotEmpty()
                .MaximumLength(200)
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Slug must contain only lowercase letters, numbers, and hyphens.");

            RuleFor(x => x.PageTitle)
                .MaximumLength(200);

            RuleFor(x => x.MetaTag)
                .MaximumLength(500);

            RuleFor(x => x.MetaDescription)
                .MaximumLength(1000);

            RuleFor(x => x.AvailableProductCount)
                .GreaterThanOrEqualTo(0);
        }
    }
}