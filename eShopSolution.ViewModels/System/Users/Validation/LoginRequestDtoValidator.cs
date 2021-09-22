using System;
using System.Collections.Generic;
using System.Text;
using eShopSolution.ViewModels.System.Users;
using FluentValidation;

namespace eShopSolution.ViewModels.System.Users.Validation
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(lr => lr.UserName).NotNull().MaximumLength(20);
            RuleFor(lr => lr.Password).NotNull().WithMessage("Không để trống")
                                      .MinimumLength(6).WithMessage("Password must be mininum 6 characters");
        }
    }
}