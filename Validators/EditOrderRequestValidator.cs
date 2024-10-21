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
                .Must(HaveValidCreditLimit)
                .WithMessage("No valid credit limit found for the order date! Please add a credit limit!");

            RuleFor(order => order)
                .Must(IsCreditLimitValid)
                .When(order => HaveValidCreditLimit(order))
                .WithMessage("Exceeding credit limit! Please add more credit for the customer!");

        }

        private bool HaveValidCreditLimit(EditOrderRequest order)
        {
            // Check if a valid credit limit exists for the order date
            var customerCreditLimit = piacomDbContext.CreditLimits
                .FirstOrDefault(cl => cl.CustomerID == order.CustomerID
                                      && cl.FromDate <= order.OrderDate
                                      && cl.ToDate >= order.OrderDate);
            return customerCreditLimit != null;
        }

        private bool IsCreditLimitValid(EditOrderRequest order)
        {
            var remainingCredit = CheckAndUpdateCreditLimit(order);

            // If remaining credit is null, it means credit limit is exceeded or not valid
            return remainingCredit != null;
        }
        private decimal? CheckAndUpdateCreditLimit(EditOrderRequest order)
        {
            var orderDate = order.OrderDate;

            // Retrieve the customer for the order
            var customer = piacomDbContext.Customers
                                          .Where(c => c.CustomerID == order.CustomerID)
                                          .FirstOrDefault();

            if (customer == null)
                return null; // If customer is not found, skip the validation (or handle it appropriately)

            // Calculate total amount of existing orders for the customer (excluding the current order)
            var totalOrderAmount = piacomDbContext.Orders
                                                  .Where(o => o.CustomerID == order.CustomerID)
                                                  .SelectMany(o => o.OrderDetails)
                                                  .Sum(od => od.TotalAmount);

            var totalPayments = piacomDbContext.Payments
                                              .Where(p => p.CustomerID == order.CustomerID)
                                              .Sum(p => p.Amount);
                                              
                                              

            // Check if the order is an update (existing order)
            var existingOrder = piacomDbContext.Orders
                                               .Where(o => o.OrderID == order.OrderID)
                                               .Include(o => o.OrderDetails)
                                               .FirstOrDefault();

            if (existingOrder != null)
            {
                // Subtract the old total of the existing order from totalOrderAmount
                var oldOrderTotalAmount = existingOrder.OrderDetails.Sum(od => od.TotalAmount);
                totalOrderAmount -= oldOrderTotalAmount;
            }


            if (order.OrderDetails != null && order.OrderDetails.Any())
            {
                var currentOrderTotalAmount = order.OrderDetails.Sum(od => od.TotalAmount);
                totalOrderAmount += currentOrderTotalAmount;
            }

            // Get customer's credit limit for the given order date
            var customerCreditLimit = piacomDbContext.CreditLimits
                                                     .Where(cl => cl.CustomerID == order.CustomerID
                                                                  && cl.FromDate <= orderDate
                                                                  && cl.ToDate >= orderDate)
                                                     .FirstOrDefault();
            if (totalOrderAmount == 0)
            {
                return customerCreditLimit?.Total; // Remaining credit is the entire credit limit
            }
            if (customerCreditLimit == null)
                return null; // No valid credit limit found for the order date (or handle appropriately)


            if (totalOrderAmount - totalPayments > customerCreditLimit.Total)
                return null; 

            // Calculate the remaining credit
            var remainingCredit = customerCreditLimit.Total - totalOrderAmount + totalPayments;

            // Return the remaining credit
            return remainingCredit;
        }

    }
}
