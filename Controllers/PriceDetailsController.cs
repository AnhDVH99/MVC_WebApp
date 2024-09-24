using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_MVC_Piacom.Data;
using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Controllers
{
    public class PriceDetailsController : Controller
    {
        private readonly PiacomDbContext _context;

        public PriceDetailsController(PiacomDbContext context)
        {
            _context = context;
        }

        // GET: PriceDetails
        public async Task<IActionResult> Index()
        {
            var piacomDbContext = _context.PriceDetails.Include(p => p.Price).Include(p => p.Product).Include(p => p.Unit);
            return View(await piacomDbContext.ToListAsync());
        }

        // GET: PriceDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceDetail = await _context.PriceDetails
                .Include(p => p.Price)
                .Include(p => p.Product)
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(m => m.PriceDetailID == id);
            if (priceDetail == null)
            {
                return NotFound();
            }

            return View(priceDetail);
        }

        // GET: PriceDetails/Create
        public IActionResult Create()
        {
            ViewBag.PriceID = new SelectList(_context.Prices, "PriceID", "PriceCode");
            ViewBag.ProductID = new SelectList(_context.Products, "ProductID", "ProductName");
            ViewBag.UnitID = new SelectList(_context.Units, "UnitID", "UnitName");
            return View();
        }

        // POST: PriceDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriceDetailID,ProductID,PriceID,VAT,EnvirontmentTax,UnitID")] PriceDetail priceDetail)
        {
            if (ModelState.IsValid)
            {
                priceDetail.PriceDetailID = Guid.NewGuid();
                _context.Add(priceDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);  // Or use logging framework
            }

            ViewBag.PriceID = new SelectList(_context.Prices, "PriceID", "PriceCode", priceDetail.PriceID);
            ViewBag.ProductID = new SelectList(_context.Products, "ProductID", "ProductName", priceDetail.ProductID);
            ViewBag.UnitID = new SelectList(_context.Units, "UnitID", "UnitName", priceDetail.UnitID);
            return View(priceDetail);
        }

        // GET: PriceDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceDetail = await _context.PriceDetails.FindAsync(id);
            if (priceDetail == null)
            {
                return NotFound();
            }
            ViewBag.PriceID = new SelectList(_context.Prices, "PriceID", "PriceCode", priceDetail.PriceID);
            ViewBag.ProductID = new SelectList(_context.Products, "ProductID", "ProductName", priceDetail.ProductID);
            ViewBag.UnitID = new SelectList(_context.Units, "UnitID", "UnitName", priceDetail.UnitID);
            return View(priceDetail);
        }

        // POST: PriceDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PriceDetailID,ProductID,PriceID,VAT,EnvirontmentTax,UnitID")] PriceDetail priceDetail)
        {
            if (id != priceDetail.PriceDetailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priceDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceDetailExists(priceDetail.PriceDetailID))
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
            ViewBag.PriceID = new SelectList(_context.Prices, "PriceID", "PriceCode", priceDetail.PriceID);
            ViewBag.ProductID = new SelectList(_context.Products, "ProductID", "ProductName", priceDetail.ProductID);
            ViewBag.UnitID = new SelectList(_context.Units, "UnitID", "UnitName", priceDetail.UnitID);
            return View(priceDetail);
        }

        // GET: PriceDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceDetail = await _context.PriceDetails
                .Include(p => p.Price)
                .Include(p => p.Product)
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(m => m.PriceDetailID == id);
            if (priceDetail == null)
            {
                return NotFound();
            }

            return View(priceDetail);
        }

        // POST: PriceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var priceDetail = await _context.PriceDetails.FindAsync(id);
            if (priceDetail != null)
            {
                _context.PriceDetails.Remove(priceDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceDetailExists(Guid id)
        {
            return _context.PriceDetails.Any(e => e.PriceDetailID == id);
        }
    }
}
