using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class AccountValidator : AbstractValidator<RegisterViewModel>
    {
        private readonly AuthDbContext authDbContext;

        public AccountValidator(AuthDbContext authDbContext) 
        {
            this.authDbContext = authDbContext;

            RuleFor(r => r.Username)
                .NotEmpty().WithMessage("Please enter User name");
                

            RuleFor(r => r.Password)
                        .NotEmpty().WithMessage("Please enter your password")
                        .Matches(new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$"))
                        .WithMessage("Password must have at least one uppercase letter, one lowercase letter, one non-alphanumeric character, and be at least 8 characters long.");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Please enter your email")
                .Must(isUnique).WithMessage("Email has existed!");
        }

        private bool isUnique(string email)
        {
            return !authDbContext.Users.Any(u => u.Email == email);
        }
    }

}
