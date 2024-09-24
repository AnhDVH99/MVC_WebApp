using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using ASP.NET_Core_MVC_Piacom.Repositories;
using Microsoft.AspNetCore.Authorization;
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

        public PriceController(IPriceRepository priceRepository, IPriceDetailRepository priceDetailRepository, IProductRepository productRepository)
        {
            this.priceRepository = priceRepository;
            this.priceDetailRepository = priceDetailRepository;
            this.productRepository = productRepository;
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
            var priceDetails = await priceDetailRepository.GetAllAsync();
            var price = new Price
            {
                PriceCode = addPriceRequest.PriceCode,
                FromDate = addPriceRequest.FromDate,
                ToDate = addPriceRequest.ToDate,
                SysU = addPriceRequest.SysU,
                SysD = DateTime.Now
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
            var products = await productRepository.GetAllAsync();
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
                PriceDetails = price.PriceDetails.ToList(),
                Products = products.Select(p => new SelectListItem
                {
                    Value = p.ProductID.ToString(),
                    Text = p.ProductName
                }).ToList()
            };

            return View(editPriceRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPriceRequest editPriceRequest)
        {

            var priceDetails = await priceDetailRepository.GetAllAsync();
            var price = new Price
            {
                PriceID = editPriceRequest.PriceID,
                PriceCode = editPriceRequest.PriceCode,
                FromDate = editPriceRequest.FromDate,
                ToDate = editPriceRequest.ToDate,
                SysU = editPriceRequest.SysU,
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
