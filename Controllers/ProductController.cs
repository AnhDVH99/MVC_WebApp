using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using ASP.NET_Core_MVC_Piacom.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IPriceDetailRepository priceDetailRepository;

        public ProductController(IProductRepository productRepository, IPriceDetailRepository priceDetailRepository)
        {
            this.productRepository = productRepository;
            this.priceDetailRepository = priceDetailRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddProductRequest addProductRequest)
        {
            var priceDetails = await priceDetailRepository.GetAllAsync();
            var product = new Product
            {
                
                ProductCode = addProductRequest.ProductCode,
                ProductDescription = addProductRequest.ProductDescription,
                ProductName = addProductRequest.ProductName,
                ProductStatus = addProductRequest.ProductStatus,
                PriceDetails = addProductRequest.PriceDetails,
                SysU = addProductRequest.SysU,
                SysD = DateTime.Now
            };
            await productRepository.AddAsync(product);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> PriceList()
        {
            var products = await productRepository.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            var product = await productRepository.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            var products = await productRepository.GetAllAsync();
            var editProductRequest = new EditProductRequest
            {
                ProductID = product.ProductID,
                ProductCode = product.ProductCode,
                ProductDescription = product.ProductDescription,
                ProductName = product.ProductName,
                ProductStatus = product.ProductStatus,
                PriceDetails = product.PriceDetails.ToList(),
                SysU = product.SysU,
            };

            return View(editProductRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductRequest editProductRequest)
        {

            var priceDetails = await priceDetailRepository.GetAllAsync();
            var product = new Product
            {
                ProductID = editProductRequest.ProductID,
                ProductCode = editProductRequest.ProductCode,
                ProductDescription = editProductRequest.ProductDescription,
                ProductName = editProductRequest.ProductName,
                ProductStatus = editProductRequest.ProductStatus,
                SysU = editProductRequest.SysU,
                SysD = DateTime.Now,
                PriceDetails = editProductRequest.PriceDetails != null
                       ? editProductRequest.PriceDetails.ToList()
                       : new List<PriceDetail>()
            };
            var updatedPriceDetails = editProductRequest.PriceDetails;
            var existingPriceDetails = product.PriceDetails.ToList();



            var updatedProduct = await productRepository.UpdateAsync(product);
            if (updatedProduct != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }
            else
            {

                // Show error notification
            }

            return RedirectToAction("Edit", new { id = editProductRequest.ProductID });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditProductRequest editProductRequest)
        {
            var deletedProduct = await productRepository.DeleteAsync(editProductRequest.ProductID);
            if (deletedProduct != null)
            {
                //show success notification
                return RedirectToAction("List");
            }

            //show error notification

            return RedirectToAction("Edit", new { id = editProductRequest.ProductID });
        }
    }
}
