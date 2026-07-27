using Application.DTOs.Category;
using Application.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Category
{
    public class CategoryAddModelValidator :AbstractValidator<CategoryAddModel>
    {// later customize theerror
        public CategoryAddModelValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().NotNull().WithMessage("category name can not be null or empty");
            RuleFor(x => x.Slug).NotEmpty().NotNull().WithMessage("sluge can not be null or empty");
       
        }
    }
}
