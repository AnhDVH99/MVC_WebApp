using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class AddCustomerRequestValidator : AbstractValidator<AddCustomerRequest>
    {
        private readonly PiacomDbContext piacomDbContext;

        public AddCustomerRequestValidator(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("Please enter your phone number")
                .Must(isUnique).WithMessage("Phone number has existed");
            
        }
        private bool isUnique(string phone)
        {
            return !piacomDbContext.Customers.Any(p => p.Phone == phone);
        }
    }

}
