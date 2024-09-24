using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using FluentValidation;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        private readonly PiacomDbContext piacomDbContext;

        public PaymentValidator(PiacomDbContext piacomDbContext ) 
        {
            this.piacomDbContext = piacomDbContext;

            RuleFor(p => p.PaymentCode)
                .NotEmpty().WithMessage("Payment code is required")
                .Must(beUniquePaymentCode).WithMessage("Payment Code has existed");
        }

        private bool beUniquePaymentCode(string paymentCode)
        {
            return !piacomDbContext.Payments.Any(p => p.PaymentCode == paymentCode);
        }
    }

}
