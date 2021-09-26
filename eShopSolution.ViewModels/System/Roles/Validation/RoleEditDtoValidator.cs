using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Roles.Validation
{
    public class RoleEditDtoValidator : AbstractValidator<RoleEditDto>
    {
        public RoleEditDtoValidator()
        {
            RuleFor(re => re.Description).MaximumLength(50).WithMessage("Mô tả phải dưới 50 kí tự")
                                         .NotNull().WithMessage("Phải nhập mô tả").WithName("Mô tả");
        }
    }
}