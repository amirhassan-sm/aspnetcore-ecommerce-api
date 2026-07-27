using Application.DTOs.Security;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Security
{
    public class UpdateMeModelValidator:AbstractValidator<UpdateMeModel>
    {
        public UpdateMeModelValidator()
        {
            RuleFor(x => x.Firstname).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.UserName).NotNull().NotEmpty().MaximumLength(50);
           
        }
    }
}
