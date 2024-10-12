using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using ASP.NET_Core_MVC_Piacom.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Validators
{
    public class EditOrderRequestValidator : AbstractValidator<EditOrderRequest>
    {
        private readonly PiacomDbContext piacomDbContext;

        public EditOrderRequestValidator(PiacomDbContext piacomDbContext) 
        {
            this.piacomDbContext = piacomDbContext;
            RuleForEach(od => od.OrderDetails).ChildRules(orderDetails =>
            {
                orderDetails.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
                orderDetails.RuleFor(x => x.priceBeforeTax)
                .GreaterThan(0).WithMessage("Please check VAT, Environment Tax or price!");
                orderDetails.RuleFor(x => x.VAT)
                .NotEmpty().WithMessage("VAT cannot be empty! (Check price details)");
                orderDetails.RuleFor(x => x.EnvironmentTax)
                .NotEmpty().WithMessage("Environment tax cannot be empty! (Check price details)");
            });

            RuleFor(order => order)
                .Must(DoesNotExceedCreditLimit)
                .WithMessage("Exceeding credit limit! Please add more credit for customer!");

            
        }

        private bool DoesNotExceedCreditLimit(EditOrderRequest order)
        {
            // Retrieve the customer for the order
            var customer = piacomDbContext.Customers
                                   .Where(c => c.CustomerID == order.CustomerID)
                                   .FirstOrDefault();

            if (customer == null)
                return true; // If customer is not found, skip the validation

            // Calculate total amount of existing orders for the customer
            var totalOrderAmount = piacomDbContext.Orders
                                           .Where(o => o.CustomerID == order.CustomerID)
                                           .SelectMany(o => o.OrderDetails)
                                           .Sum(od => od.TotalAmount);

            // Get customer's credit limit
            var customerCreditLimit = piacomDbContext.CreditLimits
                                              .Where(cl => cl.CustomerID == order.CustomerID)
                                              .OrderByDescending(cl => cl.ToDate)
                                              .FirstOrDefault()?.Total ?? 0;

            // Calculate the total amount for the current order being edited
            var currentOrderTotal = order.OrderDetails.Sum(od => od.Quantity * od.priceAfterTax);

            // Check if the total of existing orders + new order exceeds credit limit
            return (totalOrderAmount + (decimal)currentOrderTotal) <= customerCreditLimit;
        }
    }
}
