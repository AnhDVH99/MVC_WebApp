using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using ASP.NET_Core_MVC_Piacom.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{

    [Authorize(Roles = "Admin")]

    public class CreditLimitController : Controller
    {
        private readonly ICreditRepository creditRepository;
        private readonly ICustomerRepository customerRepository;

        public CreditLimitController(ICreditRepository creditRepository, ICustomerRepository customerRepository) 
        {
            this.creditRepository = creditRepository;
            this.customerRepository = customerRepository;
        }

        // GET: Orders
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get customer from repository
            var customers = await customerRepository.GetAllAsync();
            var model = new AddCreditRequest
            {
                Customers = customers.Select(c => new SelectListItem
                {
                    Text = c.CustomerName,
                    Value = c.CustomerID.ToString(),

                })
            };
            return View(model);
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddCreditRequest addCreditRequest)
        {
            var credit = new CreditLimit
            {
                CreditLimitID = addCreditRequest.CreditLimitID,
                CustomerID = addCreditRequest.CustomerID,
                FromDate = addCreditRequest.FromDate,
                ToDate = addCreditRequest.ToDate,
                CreditType = addCreditRequest.CreditType,
                OverDue = addCreditRequest.OverDue,
                Total = addCreditRequest.Total,
            };
            await creditRepository.AddAsync(credit);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> Order()
        {
            var credits = await creditRepository.GetAllAsync();
            return View(credits);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var customers = await customerRepository.GetAllAsync();
            var credit = await creditRepository.GetAsync(id);
            if (credit != null)
            {
                var editOrderRequest = new EditCreditRequest
                {
                    CreditLimitID = credit.CreditLimitID,
                    CustomerID = credit.CustomerID,
                    FromDate = credit.FromDate,
                    ToDate = credit.ToDate,
                    CreditType = credit.CreditType,
                    Total = credit.Total,
                    OverDue = credit.OverDue,
                    Customers = customers.Select(x => new SelectListItem
                    {
                        Text = x.CustomerName,
                        Value = x.CustomerID.ToString()
                       
                    })
                };
                return View(editOrderRequest);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCreditRequest editCreditRequest)
        {
            var creditLimit = new CreditLimit
            {
                CreditLimitID = editCreditRequest.CreditLimitID,
                CustomerID = editCreditRequest.CustomerID,
                FromDate = editCreditRequest.FromDate,
                ToDate = editCreditRequest.ToDate,
                CreditType = editCreditRequest.CreditType,
                Total = editCreditRequest.Total,
                OverDue = editCreditRequest.OverDue
            };
            var updateCreditLimit = await creditRepository.UpdateAsync(creditLimit);
            if (updateCreditLimit != null)
            {
                // Show success notification
                TempData["SuccessMessage"] = "Credit limit updated successfully.";
                return RedirectToAction("List");
            }
            else
            {
                // Show error notification
                TempData["ErrorMessage"] = "Something went wrong.";
            }

            return RedirectToAction("Edit", new { id = editCreditRequest.CreditLimitID });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditCreditRequest editCreditRequest)
        {
            var deletedCredit = await creditRepository.DeleteAsync(editCreditRequest.CreditLimitID);
            if (deletedCredit != null)
            {
                //show success notification
                return RedirectToAction("List");
            }

            //show error notification

            return RedirectToAction("Edit", new { id = editCreditRequest.CreditLimitID });
        }
    }
}
    

