using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    [Authorize(Roles = "Admin")]

    public class OrderDetailsController : Controller
    {
        private readonly PiacomDbContext _context;

        public OrderDetailsController(PiacomDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var piacomDbContext = _context.OrderDetails.Include(o => o.Order).Include(o => o.Unit).Include(o =>o.Products);
            return View(await piacomDbContext.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Unit)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderDate");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductCode");
            ViewData["PriceDetailID"] = new SelectList(_context.PriceDetails, "PriceDetailID", "VAT");
            ViewData["UnitID"] = new SelectList(_context.Units, "UnitID", "UnitCode");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailID,OrderID,ProductID,UnitID,Quantity,PriceDetailID,Price,Discount,TotalAmount,DueDate")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                orderDetail.OrderDetailID = Guid.NewGuid();
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", orderDetail.ProductID);
            ViewData["PriceDetailID"] = new SelectList(_context.PriceDetails, "PriceDetailID", "PriceDetailID",orderDetail.PriceDetailID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderID", orderDetail.OrderID);
            ViewData["UnitID"] = new SelectList(_context.Units, "UnitID", "UnitID", orderDetail.UnitID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductCode", orderDetail.ProductID);
            ViewData["PriceDetailID"] = new SelectList(_context.PriceDetails, "PriceDetailID", "VAT", orderDetail.PriceDetailID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderDate", orderDetail.OrderID);
            ViewData["UnitID"] = new SelectList(_context.Units, "UnitID", "UnitCode", orderDetail.UnitID);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OrderDetailID,OrderID,ProductID,UnitID,Quantity,PriceDetailID,Price,Discount,TotalAmount,DueDate")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderDetailID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductCode", orderDetail.ProductID);
            ViewData["PriceDetailID"] = new SelectList(_context.PriceDetails, "PriceDetailID", "PriceDetailID", orderDetail.PriceDetailID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderDate", orderDetail.OrderID);
            ViewData["UnitID"] = new SelectList(_context.Units, "UnitID", "UnitID", orderDetail.UnitID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Unit)
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(Guid id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailID == id);
        }
    }
}
