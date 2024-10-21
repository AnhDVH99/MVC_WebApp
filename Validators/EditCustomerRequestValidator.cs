using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class EditCustomerRequestValidator : AbstractValidator<EditCustomerRequest>
    {
        private readonly PiacomDbContext piacomDbContext;

        public EditCustomerRequestValidator(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;

            RuleForEach(x => x.CreditLimits).ChildRules(creditLimit =>
            {
                creditLimit.RuleFor(x => x.ToDate)
                           .GreaterThan(x => x.FromDate)
                           .WithMessage("'To Date' must be after the 'From Date'.");
                creditLimit.RuleFor(x => new { x.FromDate, x.ToDate, x.CustomerID })
                           .Must(dates => DateRangeExisted(dates.FromDate, dates.ToDate, dates.CustomerID))
                           .WithMessage("Date range has existed!");
            });
            

        }
        private bool DateRangeExisted(DateTime fromDate, DateTime toDate, Guid customerId)
        {
            return !piacomDbContext.CreditLimits
                .Where(cl=> cl.CustomerID == customerId)
                .Any(cl => fromDate <= cl.ToDate && toDate >= cl.FromDate);
        }
    }
}
