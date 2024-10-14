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

            RuleFor(u => new { u.Username, u.Password })
                .Must(login => ValidateUserCredentials(login.Username, login.Password))
                .WithMessage("Invalid username or password");

        }
        private bool ValidateUserCredentials(string username, string password)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null) return false;

            var validPassword = _userManager.CheckPasswordAsync(user, password).Result;
            return validPassword;
        }
    }
}
