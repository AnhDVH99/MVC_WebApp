using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginViewModelValidator(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Please enter user name");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Please enter password");

           
        }
    }
}
