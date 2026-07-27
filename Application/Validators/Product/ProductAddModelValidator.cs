using Application.DTOs.Product;
using FluentValidation;

namespace Application.Validators.Product
{
    public class ProductAddModelValidator : AbstractValidator<ProductAddModel>
    {
        public ProductAddModelValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ProductDescription)
                .MaximumLength(5000);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.BasePrice)
                .GreaterThan(0);

            RuleFor(x => x.ImageUrl)
                .MaximumLength(1000);

            RuleFor(x => x.Introduction)
                .MaximumLength(3000);

            RuleFor(x => x.AvailableProductCount)
                .GreaterThanOrEqualTo(0);
        }
    }
}