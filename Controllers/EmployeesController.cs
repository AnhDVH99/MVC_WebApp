using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using ASP.NET_Core_MVC_Piacom.Models.ViewModels;
using ASP.NET_Core_MVC_Piacom.Repositories;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }


        // GET: Orders
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get customer from repository
            var employees = await employeeRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddEmployeeRequest addEmployeeRequest)
        {
            var employee = new Employee
            {
                EmployeeID = addEmployeeRequest.EmployeeID,
                FirstName = addEmployeeRequest.FirstName,
                LastName = addEmployeeRequest.LastName,
                Email = addEmployeeRequest.Email,
                Phone = addEmployeeRequest.Phone,
                JobTitle = addEmployeeRequest.JobTitle
            };
            await employeeRepository.AddAsync(employee);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> Employee()
        {
            var employees = await employeeRepository.GetAllAsync();
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            var employee = await employeeRepository.GetAsync(id);
            if (employee != null)
            {
                var editEmployeeRequest = new EditEmployeeRequest
                {
                    EmployeeID = employee.EmployeeID,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    JobTitle = employee.JobTitle,
                };
                return View(editEmployeeRequest);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeRequest editEmployeeRequest)
        {
            var employee = new Employee
            {
                EmployeeID = editEmployeeRequest.EmployeeID,
                FirstName = editEmployeeRequest.FirstName,
                LastName = editEmployeeRequest.LastName,
                Email = editEmployeeRequest.Email,
                Phone = editEmployeeRequest.Phone,
                JobTitle = editEmployeeRequest.JobTitle,
            };
            var updatedEmployee = await employeeRepository.UpdateAsync(employee);
            if (updatedEmployee != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }
            else
            {
                // Show error notification
            }

            return RedirectToAction("Edit", new { id = editEmployeeRequest.EmployeeID });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditEmployeeRequest editEmployeeRequest)
        {
            var deletedEmployee = await employeeRepository.DeleteAsync(editEmployeeRequest.EmployeeID);
            if (deletedEmployee != null)
            {
                //show success notification
                return RedirectToAction("List");
            }

            //show error notification

            return RedirectToAction("Edit", new { id = editEmployeeRequest.EmployeeID });
        }
    }
}


