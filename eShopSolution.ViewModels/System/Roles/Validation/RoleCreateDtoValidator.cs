using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Roles.Validation
{
    internal class RoleCreateDtoValidator : AbstractValidator<RoleCreateDto>
    {
        public RoleCreateDtoValidator()
        {
            RuleFor(re => re.Description).MaximumLength(50).WithMessage("Mô tả phải dưới 50 kí tự")
                                         .NotNull().WithMessage("Phải nhập mô tả").WithName("Mô tả");
            RuleFor(re => re.Name).MinimumLength(4).WithMessage("Tên User từ 4 kí tự")
                                  .NotNull().WithMessage("Không được để trống").WithName("Vai trò");
        }
    }
}