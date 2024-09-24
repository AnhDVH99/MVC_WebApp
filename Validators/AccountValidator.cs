using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class AccountValidator : AbstractValidator<RegisterViewModel>
    {
        private readonly AuthDbContext authDbContext;

        public AccountValidator(AuthDbContext authDbContext) 
        {
            this.authDbContext = authDbContext;

            RuleFor(r => r.Username)
                .NotEmpty().WithMessage("Please enter User name")
                .Must(isUnique).WithMessage("User name has existed");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Please enter password");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Please enter email");
        }

        private bool isUnique(string username)
        {
            return !authDbContext.Users.Any(u => u.UserName == username);
        }
    }

}
