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

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    [Authorize(Roles = "Admin")]

    public class OrdersController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IEmployeeRepository employeeRepository;

        public OrdersController(IOrderRepository orderRepository, ICustomerRepository customerRepository, IEmployeeRepository employeeRepository)
        {
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.employeeRepository = employeeRepository;
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
                SysU = User.Identity.Name
            };
            await orderRepository.AddAsync(order);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> Order()
        {
            var orders = await orderRepository.GetAllAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
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
                    SysU = User.Identity.Name,
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
                    })
                };
                return View(editOrderRequest);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditOrderRequest editOrderRequest)
        {
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
                SysU = User.Identity.Name,
                SysD = DateTime.Now,
            };
            var updatedOrder = await orderRepository.UpdateAsync(order);
            if (updatedOrder != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }
            else
            {
                // Show error notification
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
    }
}
