using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class EditCustomerRequestValidator : AbstractValidator<EditCustomerRequest>
    {
        private readonly PiacomDbContext piacomDbContext;

        public EditCustomerRequestValidator(PiacomDbContext piacomDbContext)
        {

            RuleForEach(x => x.CreditLimits).ChildRules(creditLimit =>
            {
                creditLimit.RuleFor(x => x.ToDate)
                    .GreaterThan(x => x.FromDate)
                    .WithMessage("'To Date' must be after the 'From Date'.");

                creditLimit.RuleFor(c => c.CreditLimitID)
               .Must(id => id == Guid.Empty || id != Guid.Empty) // Accept empty or valid IDs
               .WithMessage("Credit Limit ID must be valid if provided.");
            });
            this.piacomDbContext = piacomDbContext;
        }

        private bool IsValidId(string id)
        {
            // Check if the ID is valid (for example, check against the database)
            return piacomDbContext.CreditLimits.Any(cl => cl.CreditLimitID.ToString() == id);
        }
    }
}
