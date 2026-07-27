using Application.DTOs.Security.Roles;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Security
{
    public class UpdateRoleDtoValidator:AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator()
        {
            RuleFor(x => x.RoleId).GreaterThan(0).NotNull().NotEmpty();
            RuleFor(x => x.RoleName).NotNull().NotEmpty();
        }
    }
}
