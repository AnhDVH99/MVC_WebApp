using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class AddPriceValidator : AbstractValidator<AddPriceRequest>
    {
        private readonly PiacomDbContext piacomDbContext;

        public AddPriceValidator(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;

            RuleFor(p => p.PriceCode)
                .Must(PriceCodeExisted)
                .WithMessage("PriceCode has existed");

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate)
                .WithMessage("From date must be less than or equal To date");

            RuleFor(x => new { x.FromDate, x.ToDate })
                .Must(dates => !DateRangeExisted(dates.FromDate, dates.ToDate))
                .WithMessage("This date range already existed");
        }

        private bool DateRangeExisted(DateTime fromDate, DateTime toDate)
        {
            return piacomDbContext.Prices.Any(p => (p.FromDate <= toDate && p.ToDate >= fromDate));
        }

        private bool PriceCodeExisted(string priceCode)
        {
            return !piacomDbContext.Prices.Any(p => p.PriceCode == priceCode);
        }
    }
}
