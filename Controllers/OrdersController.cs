using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Repositories;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    [Authorize(Roles = "Admin")]

    public class OrdersController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUnitRepository unitRepository;
        private readonly IProductRepository productRepository;

        public OrdersController(IOrderRepository orderRepository, ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository, UserManager<IdentityUser> userManager,
            IUnitRepository unitRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.employeeRepository = employeeRepository;
            this.userManager = userManager;
            this.unitRepository = unitRepository;
            this.productRepository = productRepository;
        }

        // GET: Orders
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get customer from repository
            var employees = await employeeRepository.GetAllAsync();
            var customers = await customerRepository.GetAllAsync();
            var model = new AddOrderRequest
            {

                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays(7),
                ShippedDate = null,
                Employees = employees.Select(e => new SelectListItem
                {
                    Text = e.FirstName,
                    Value = e.EmployeeID.ToString()

                }),
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
        public async Task<IActionResult> Add(AddOrderRequest addOrderRequest)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var order = new Order
            {
                OrderDate = addOrderRequest.OrderDate,
                RequiredDate = addOrderRequest.RequiredDate,
                ShippedDate = addOrderRequest.ShippedDate,
                OrderStatus = addOrderRequest.OrderStatus,
                Comment = addOrderRequest.Comment,
                CustomerID = addOrderRequest.CustomerID,
                EmployeeID = addOrderRequest.EmployeeID,
                SysD = DateTime.Now,
                SysU = currentUser?.UserName
            };
            await orderRepository.AddAsync(order);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> Order(string sortOrder, string searchTerm)
        {
            var orders = await GetSortedOrders(sortOrder, searchTerm);
            return View(orders);
        }

        
        [HttpGet]
        public async Task<IActionResult> OrderSearch(string sortOrder, string searchTerm)
        {
            var orders = await GetSortedOrders(sortOrder, searchTerm);

            return PartialView("OrderListPartial", orders);
        }

        private async Task<List<Order>> GetSortedOrders(string sortOrder, string searchTerm)
        {
            ViewData["OrderDateSort"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["CurrentSort"] = sortOrder;
            ViewData["searchTerm"] = searchTerm;

            // Retrieve orders, applying search if needed
            IEnumerable<Order> orders;
            if (!String.IsNullOrEmpty(searchTerm))
            {
                orders = await orderRepository.SearchOrdersAsync(searchTerm);
            }
            else
            {
                orders = await orderRepository.GetAllOrder();
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.OrderDate);
                    break;
                default:
                    orders = orders.OrderBy(o => o.OrderDate);
                    break;
            }

            return orders.ToList();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productList = await productRepository.GetAllAsync();
            var products = productList.Select(p => new SelectListItem
            {
                Value = p.ProductID.ToString(),
                Text = p.ProductName,
                
            }).ToList();
            var unitList = await unitRepository.GetAllAsync();
            var units = unitList.Select(u => new SelectListItem
            {
                Value = u.UnitID.ToString(),
                Text = u.UnitName
            }).ToList();
            var currentUser = userManager.GetUserAsync(User);
            var employees = await employeeRepository.GetAllAsync();
            var customers = await customerRepository.GetAllAsync();
            var order = await orderRepository.GetAsync(id);
            if (order != null)
            {
                var editOrderRequest = new EditOrderRequest
                {
                    OrderID = order.OrderID,
                    OrderDate = order.OrderDate,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    OrderStatus = order.OrderStatus,
                    Comment = order.Comment,
                    CustomerID = order.CustomerID,
                    EmployeeID = order.EmployeeID,
                    SysU = order.SysU,
                    SysD = DateTime.Now,
                    Employees = employees.Select(e => new SelectListItem
                    {
                        Text = e.FirstName,
                        Value = e.EmployeeID.ToString()

                    }),
                    Customers = customers.Select(x => new SelectListItem
                    {
                        Text = x.CustomerName,
                        Value = x.CustomerID.ToString(),
                        Selected = (x.CustomerID == order.CustomerID)
                    }),
                    OrderDetails = order.OrderDetails.ToList(),
                    Products = products,
                    Units = units
                };

                return View(editOrderRequest);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditOrderRequest editOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        // Print to console (or use Debug.WriteLine if you prefer)
                        Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                    }
                }

                var productList = await productRepository.GetAllAsync();
                var products = productList.Select(p => new SelectListItem
                {
                    Value = p.ProductID.ToString(),
                    Text = p.ProductName
                }).ToList();

                var unitList = await unitRepository.GetAllAsync();
                var units = unitList.Select(u => new SelectListItem
                {
                    Value = u.UnitID.ToString(),
                    Text = u.UnitName
                }).ToList();

                var employees = await employeeRepository.GetAllAsync();
                var customers = await customerRepository.GetAllAsync();

                // Populate the existing order details
                var orders = await orderRepository.GetAsync(editOrderRequest.OrderID);
                if (orders != null)
                {
                    // Repopulate editOrderRequest with current values and lists
                    editOrderRequest.Employees = employees.Select(e => new SelectListItem
                    {
                        Text = e.FirstName,
                        Value = e.EmployeeID.ToString()
                    });

                    editOrderRequest.Customers = customers.Select(x => new SelectListItem
                    {
                        Text = x.CustomerName,
                        Value = x.CustomerID.ToString(),
                        Selected = (x.CustomerID == orders.CustomerID)
                    });

                    editOrderRequest.Products = products;
                    editOrderRequest.Units = units;
                   
                }

                // Return the view with the model including validation errors
                return View(editOrderRequest);
            }
            var currentUser = await userManager.GetUserAsync(User);
            var order = new Order
            {
                OrderID = editOrderRequest.OrderID,
                OrderDate = editOrderRequest.OrderDate,
                RequiredDate = editOrderRequest.RequiredDate,
                ShippedDate = editOrderRequest.ShippedDate,
                OrderStatus = editOrderRequest.OrderStatus,
                Comment = editOrderRequest.Comment,
                CustomerID = editOrderRequest.CustomerID,
                EmployeeID = editOrderRequest.EmployeeID,
                SysU = currentUser?.UserName,
                SysD = DateTime.Now,
                OrderDetails = editOrderRequest.OrderDetails != null
                       ? editOrderRequest.OrderDetails.ToList()
                       : new List<OrderDetail>()
            };
            var updatedOrder = await orderRepository.UpdateAsync(order);
            if (updatedOrder != null)
            {
                TempData["SuccessMessage"] = "Order updated successfully!";
                RedirectToAction("Edit", new { id = editOrderRequest.OrderID });
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update the order.";
            }

            return RedirectToAction("Edit", new { id = editOrderRequest.OrderID });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditOrderRequest editOrderRequest)
        {
            var deletedOrder = await orderRepository.DeleteAsync(editOrderRequest.OrderID);
            if (deletedOrder != null)
            {
                //show success notification
                return RedirectToAction("List");
            }

            //show error notification

            return RedirectToAction("Edit", new { id = editOrderRequest.OrderID });
        }



        [Route("Orders/GetProductPriceDetail")]
        [HttpGet]
        public async Task<IActionResult> GetProductPriceDetail(Guid productId, DateTime orderDate)
        {
            var priceDetail = await orderRepository.GetProductPriceDetailByOrderAsync(productId, orderDate);

            if (priceDetail == null)
            {
                return NotFound();
            }

            // Return relevant price detail information as JSON
            return Json(new
            {
                price = priceDetail.Price,
                vat = priceDetail.VAT,
                environmentTax = priceDetail.EnvirontmentTax,
                priceBeforeTax = priceDetail.PriceBeforeTax, // if you calculate it separately
            });
        }


        public async Task<IActionResult> Clone(Guid id)
        {
            var existingOrder = await orderRepository.GetAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }
            var currentUser = await userManager.GetUserAsync(User);
            // Create a new order based on the existing one
            var newOrder = new Order
            {
                // Clone properties as needed, ensuring you set new values for fields like OrderID
                CustomerID = existingOrder.CustomerID,
                OrderDate = DateTime.Now, // Set current date or modify as needed
                RequiredDate = existingOrder.RequiredDate,
                OrderStatus = existingOrder.OrderStatus,
                Comment = existingOrder.Comment,
                SysU = existingOrder.SysU,
                SysD = existingOrder.SysD,
                
            };

            // Save the new order to the database
            await orderRepository.AddAsync(newOrder);

            // Redirect to the order list or detail view
            return RedirectToAction("List");
        }
    }
}
