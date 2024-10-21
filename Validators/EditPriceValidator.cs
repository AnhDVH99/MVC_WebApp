using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class EditPriceValidator : AbstractValidator<EditPriceRequest>
    {
        private readonly PiacomDbContext piacomDbContext;

        public EditPriceValidator(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;


            RuleFor(x => new { x.PriceCode, x.PriceID })
                .Must(prices => PriceCodeExisted(prices.PriceCode, prices.PriceID))
                .WithMessage("Price code has existed");

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate)
                .WithMessage("From date must be less than or equal To date");

            RuleFor(x => new { x.FromDate, x.ToDate, x.PriceID })
                .Must(dates => !DateRangeExisted(dates.FromDate, dates.ToDate, dates.PriceID))
                .WithMessage("This date range already existed");
        }

        private bool DateRangeExisted(DateTime fromDate, DateTime toDate, Guid priceId)
        {
            return piacomDbContext.Prices.Any(p => p.PriceID != priceId && (p.FromDate <= toDate && p.ToDate >= fromDate));
        }

        private bool PriceCodeExisted(string priceCode, Guid priceId)
        {
            return !piacomDbContext.Prices.Any(p => p.PriceID != priceId && p.PriceCode == priceCode);
        }

    }
}
