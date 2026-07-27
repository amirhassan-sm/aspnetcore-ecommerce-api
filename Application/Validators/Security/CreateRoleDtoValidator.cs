using Application.DTOs.Security.Roles;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Security
{
    public class CreateRoleDtoValidator:AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().NotNull();
        }
    }
}
