using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using FluentValidation;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        private readonly PiacomDbContext piacomDbContext;

        public PaymentValidator(PiacomDbContext piacomDbContext)
        {
            this.piacomDbContext = piacomDbContext;

            RuleFor(p => new { p.PaymentCode , p.PaymentID } )
                .NotEmpty().WithMessage("Payment code is required")
                .Must(payments => beUniquePaymentCode(payments.PaymentCode, payments.PaymentID))
                .WithMessage("Payment Code has existed");
        }

        private bool beUniquePaymentCode(string paymentCode, Guid paymentId)
        {
            return !piacomDbContext.Payments.Any(p => p.PaymentID != paymentId && p.PaymentCode == paymentCode);
        }
    }

}
