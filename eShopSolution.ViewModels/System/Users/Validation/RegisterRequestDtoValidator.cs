using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users.Validation
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(rr => rr.FirstName).MaximumLength(10).WithMessage("Firstname is less than 10 character")
                                        .NotNull().WithMessage("Not empty field");

            RuleFor(rr => rr.UserName).MinimumLength(6).WithMessage("UserName is morthan 6 charactoer")
                                        .NotNull().WithMessage("Not null this field");
            RuleFor(rr => rr.Email).NotNull().WithMessage("Not empty field")
                                    .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Mail Format not true");
            RuleFor(rr => rr.PhoneNumber).Matches(@"\d{10}").WithMessage("Phone format not true");

            //RuleFor(rr => rr).Custom((fn, context) =>
            //{
            //    if (fn.FirstName.Contains("an"))
            //    {
            //        context.AddFailure("usernameis forbiden");
            //    }
            //});
        }
    }
}