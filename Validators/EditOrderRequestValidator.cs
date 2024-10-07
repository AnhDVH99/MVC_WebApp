using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using FluentValidation;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class EditOrderRequestValidator : AbstractValidator<EditOrderRequest>
    {
        public EditOrderRequestValidator() 
        {
            RuleForEach(od => od.OrderDetails).ChildRules(orderDetails =>
            {
                orderDetails.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            });
        }
    }
}
