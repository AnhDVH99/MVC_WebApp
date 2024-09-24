using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using ASP.NET_Core_MVC_Piacom.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ASP.NET_Core_MVC_Piacom.Controllers;
[Authorize(Roles = "Admin")]

public class CustomerController : Controller
{
    private readonly ICustomerRepository customerRepository;
    private readonly IEmployeeRepository employeeRepository;
    private readonly ICreditRepository creditRepository;

    public CustomerController(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, ICreditRepository creditRepository)
    {
        this.customerRepository = customerRepository;
        this.employeeRepository = employeeRepository;
        this.creditRepository = creditRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var employees = await employeeRepository.GetAllAsync();
        var creditLimits = await creditRepository.GetAllAsync();
        var model = new AddCustomerRequest
        {
            Employees = employees.Select(e => new SelectListItem
            {
                Text = e.FirstName,
                Value = e.EmployeeID.ToString()
            }),
        };
        return View(model);
    }

    [HttpPost]
    [ActionName("Add")]
    public async Task<IActionResult> Add(AddCustomerRequest addCustomerRequest)
    {

        if (!ModelState.IsValid)
        {
            var employees = await employeeRepository.GetAllAsync();
            addCustomerRequest.Employees = employees.Select(e => new SelectListItem
            {
                Text = e.FirstName,
                Value = e.EmployeeID.ToString()
            });
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                               .Select(e => e.ErrorMessage)
                               .ToList();

            // Optionally log the errors or inspect them in your debugger
            return View(addCustomerRequest);
        }
        var customer = new Customer
        {
            CustomerName = addCustomerRequest.CustomerName,
            Phone = addCustomerRequest.Phone,
            AddressLine1 = addCustomerRequest.Address1,
            AddressLine2 = addCustomerRequest.Address2,
            City = addCustomerRequest.City,
            State = addCustomerRequest.State,
            PostalCode = addCustomerRequest.PostalCode,
            Country = addCustomerRequest.Country,
            SaleRepEmployeeID = addCustomerRequest.SaleRepEmployeeID,
            CreditLimitID = Guid.NewGuid(),
        };
        await customerRepository.AddAsync(customer);

        return RedirectToAction("List");
    }

    [HttpGet]
    [ActionName("List")]
    public async Task<IActionResult> CustomerList()
    {
        var customers = await customerRepository.GetAllAsync();
        return View(customers);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {

        var customer = await customerRepository.GetAsync(id);

        if (customer == null)
        {
            return NotFound();
        }
        var employees = await employeeRepository.GetAllAsync();
        var editCustomerRequest = new EditCustomerRequest
        {
            CustomerID = customer.CustomerID,
            CustomerName = customer.CustomerName,
            Address1 = customer.AddressLine1,
            Address2 = customer.AddressLine2,
            City = customer.City,
            Country = customer.Country,
            CreditLimitID = customer.CreditLimitID,
            Phone = customer.Phone,
            PostalCode = customer.PostalCode,
            SaleRepEmployeeID = customer.SaleRepEmployeeID,
            State = customer.State,
            Employees = employees.Select(e => new SelectListItem
            {
                Text = e.FirstName,
                Value = e.EmployeeID.ToString()
            }),
            CreditLimits = customer.CreditLimits.ToList()
        };

        return View(editCustomerRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditCustomerRequest editCustomerRequest)
    {
        var customer = new Customer
        {
            CustomerID = editCustomerRequest.CustomerID,
            CustomerName = editCustomerRequest.CustomerName,
            Phone = editCustomerRequest.Phone,
            AddressLine1 = editCustomerRequest.Address1,
            AddressLine2 = editCustomerRequest.Address2,
            City = editCustomerRequest.City,
            State = editCustomerRequest.State,
            PostalCode = editCustomerRequest.PostalCode,
            Country = editCustomerRequest.Country,
            SaleRepEmployeeID = editCustomerRequest.SaleRepEmployeeID,
            CreditLimitID = editCustomerRequest.CreditLimitID,
            CreditLimits = editCustomerRequest.CreditLimits != null
                   ? editCustomerRequest.CreditLimits.ToList()
                   : new List<CreditLimit>()
        };
        var updatedCreditLimits = editCustomerRequest.CreditLimits;
        var existingCreditLimits = customer.CreditLimits.ToList();

        var updatedCustomer = await customerRepository.UpdateAsync(customer);
        if (updatedCustomer != null)
        {
            // Show success notification
            return RedirectToAction("List");
        }
        else
        {

            // Show error notification
        }

        return RedirectToAction("Edit", new { id = editCustomerRequest.CustomerID });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditCustomerRequest editCustomerRequest)
    {
        var deletedCustomer = await customerRepository.DeleteAsync(editCustomerRequest.CustomerID);
        if (deletedCustomer != null)
        {
            //show success notification
            return RedirectToAction("List");
        }

        //show error notification

        return RedirectToAction("Edit", new { id = editCustomerRequest.CustomerID });
    }
}