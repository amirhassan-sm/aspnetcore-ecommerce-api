using Application.DTOs.Security;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Security
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Password).NotNull().NotEmpty().MaximumLength(30);
        }

    }
}
