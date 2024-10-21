using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using FluentValidation;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        private readonly PiacomDbContext piacomDbContext;

        public ProductValidator(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;

            RuleFor(p => new { p.ProductCode, p.ProductID })
            .NotEmpty().WithMessage("Product code is required")
            .Must(payments => beUniquePaymentCode(payments.ProductCode, payments.ProductID))
            .WithMessage("Product Code has existed");
        }
        private bool beUniquePaymentCode(string productCode, Guid productId)
        {
            return !piacomDbContext.Products.Any(p => p.ProductID != productId && p.ProductCode == productCode);
        }

    }
}
