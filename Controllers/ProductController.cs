using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using ASP.NET_Core_MVC_Piacom.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IPriceDetailRepository priceDetailRepository;
        private readonly UserManager<IdentityUser> userManager;

        public ProductController(IProductRepository productRepository, IPriceDetailRepository priceDetailRepository, UserManager<IdentityUser> userManager )
        {
            this.productRepository = productRepository;
            this.priceDetailRepository = priceDetailRepository;
            this.userManager = userManager;
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
            var currentUser = await userManager.GetUserAsync(User);
            var product = new Product
            {
                
                ProductCode = addProductRequest.ProductCode,
                ProductDescription = addProductRequest.ProductDescription,
                ProductName = addProductRequest.ProductName,
                ProductStatus = addProductRequest.ProductStatus,
                SysU = currentUser?.UserName,
                SysD = DateTime.Now
            };
            await productRepository.AddAsync(product);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetails(Guid productId)
        {
            var priceDetail = await priceDetailRepository.GetAsync(productId);

            if (priceDetail == null)
            {
                return NotFound();
            }

            // Return VAT and environment tax as JSON
            return Json(new
            {
                VAT = priceDetail.VAT,
                EnvironmentTax = priceDetail.EnvirontmentTax
            });
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

            var currentUser = await userManager.GetUserAsync(User);
            var product = await productRepository.GetAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            var editProductRequest = new EditProductRequest
            {
                ProductID = product.ProductID,
                ProductCode = product.ProductCode,
                ProductDescription = product.ProductDescription,
                ProductName = product.ProductName,
                ProductStatus = product.ProductStatus,
                SysU = product.SysU,
            };

            return View(editProductRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductRequest editProductRequest)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var priceDetails = await priceDetailRepository.GetAllAsync();
            var product = new Product
            {
                ProductID = editProductRequest.ProductID,
                ProductCode = editProductRequest.ProductCode,
                ProductDescription = editProductRequest.ProductDescription,
                ProductName = editProductRequest.ProductName,
                ProductStatus = editProductRequest.ProductStatus,
                SysU = currentUser?.UserName,
                SysD = DateTime.Now,
            };

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
