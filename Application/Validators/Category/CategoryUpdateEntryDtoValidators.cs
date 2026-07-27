using Application.DTOs.Category;
using FluentValidation;

public class CategoryUpdateEntryDtoValidators : AbstractValidator<CategoryUpdateEntryDto>
{
    public CategoryUpdateEntryDtoValidators()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.CategoryDescription)
            .MaximumLength(1000);

        RuleFor(x => x.ParentId)
            .GreaterThan(0)
            .When(x => x.ParentId.HasValue);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(200)
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must contain only lowercase letters, numbers, and hyphens.");
    }
}