using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using ASP.NET_Core_MVC_Piacom.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    [Authorize(Roles = "Admin")]

    public class PriceController : Controller
    {
        private readonly IPriceRepository priceRepository;
        private readonly IPriceDetailRepository priceDetailRepository;
        private readonly IProductRepository productRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUnitRepository unitRepository;

        public PriceController(IPriceRepository priceRepository, IPriceDetailRepository priceDetailRepository,
            IProductRepository productRepository, UserManager<IdentityUser> userManager, IUnitRepository unitRepository )
        {
            this.priceRepository = priceRepository;
            this.priceDetailRepository = priceDetailRepository;
            this.productRepository = productRepository;
            this.userManager = userManager;
            this.unitRepository = unitRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddPriceRequest addPriceRequest)
        {
           var currentUser = await userManager.GetUserAsync(User);
            var price = new Price
            {
                PriceCode = addPriceRequest.PriceCode,
                FromDate = addPriceRequest.FromDate,
                ToDate = addPriceRequest.ToDate,
                SysU = currentUser?.UserName,
                SysD = DateTime.Now,
               
            };
            var createdPrice = await priceRepository.AddAsync(price);
            return RedirectToAction("Edit", new { id = createdPrice.PriceID });
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> PriceList()
        {
            var prices = await priceRepository.GetAllAsync();
            return View(prices);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            var price = await priceRepository.GetAsync(id);
            var productList = await productRepository.GetAllAsync();
            var unitList = await unitRepository.GetAllAsync();
            var products = productList.Select(p => new SelectListItem
            {
                Value = p.ProductID.ToString(),
                Text = p.ProductName
            }).ToList();
            var units = unitList.Select(u => new SelectListItem
            {
                Value = u.UnitID.ToString(),
                Text = u.UnitName
            });
            if (price == null)
            {
                return NotFound();
            }
            var prices = await priceRepository.GetAllAsync();
            var editPriceRequest = new EditPriceRequest
            {
                PriceID = price.PriceID,
                PriceCode = price.PriceCode,
                FromDate = price.FromDate,
                ToDate = price.ToDate,
                SysU = price.SysU,
                SysD = DateTime.Now,
                PriceDetails = price.PriceDetails?.ToList(),
                Products = products,
                Units = units

            };

            return View(editPriceRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPriceRequest editPriceRequest)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    var errors = state.Value.Errors;
                    foreach (var error in errors)
                    {
                        // Log the key and error message
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
                return RedirectToAction("Edit", new { id = editPriceRequest.PriceID });
            }
            var price = new Price
            {   
                PriceID = editPriceRequest.PriceID,
                PriceCode = editPriceRequest.PriceCode,
                FromDate = editPriceRequest.FromDate,
                ToDate = editPriceRequest.ToDate,
                SysU = currentUser?.UserName,
                SysD = DateTime.Now,
                PriceDetails = editPriceRequest.PriceDetails != null
                       ? editPriceRequest.PriceDetails.ToList()
                       : new List<PriceDetail>()
            };
            var updatedPriceDetails = editPriceRequest.PriceDetails;
            var existingPriceDetails = price.PriceDetails.ToList();

            var updatedPrice = await priceRepository.UpdateAsync(price);
            if (updatedPrice != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }
            else
            {

                // Show error notification
            }

            return RedirectToAction("Edit", new { id = editPriceRequest.PriceID });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditPriceRequest editPriceRequest)
        {
            var deletedCustomer = await priceRepository.DeleteAsync(editPriceRequest.PriceID);
            if (deletedCustomer != null)
            {
                //show success notification
                return RedirectToAction("List");
            }

            //show error notification

            return RedirectToAction("Edit", new { id = editPriceRequest.PriceID });
        }
    }
}
