using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISTPLR_one;

namespace ISTPLR_one.Controllers
{
    public class PositionsController : Controller
    {
        private readonly ISTPContext _context;

        public PositionsController(ISTPContext context)
        {
            _context = context;
        }

        // GET: Positions
        public async Task<IActionResult> Index(int id, bool isToday)
        {
            ViewBag.OrderId = id;
            ViewBag.isToday = isToday;
            var neededOrder = _context.Orders.Where(o => o.OrderId == id).Include(o => o.Bonus);
            ViewBag.OrderDate = neededOrder.Select(o => o.Date).FirstOrDefault().ToShortDateString();
            ViewBag.OrderCoffeeShop = neededOrder.Select(o => o.CoffeeShopId).FirstOrDefault();
            var PositionsInOrder = _context.Positions.Where(b => b.OrderId == id).Include(b=>b.Order).Include(b=>b.Product);
            var disc = neededOrder.Select(o => o.Bonus.FavoriteProductId).FirstOrDefault();
            ViewBag.Discount = disc;    
            return View(await PositionsInOrder.ToListAsync());

        }

        // GET: Positions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: Positions/Create
        public IActionResult Create(int orderId)
        {
            ViewBag.OrderId = orderId;
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PositionId,ProductId,Quantity,OrderId")] Position position)
        {
            //position.OrderId = orderId;
            if (ModelState.IsValid)
            {
                _context.Add(position);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Positions", new { id = position.OrderId, isToday = true });
            }
            
            return View(position);
        }

        // GET: Positions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", position.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", position.ProductId);
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PositionId,ProductId,Quantity,OrderId")] Position position)
        {
            if (id != position.PositionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(position);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(position.PositionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Positions", new { id = position.OrderId, isToday = true });
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", position.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", position.ProductId);
            return View(position);
        }

        // GET: Positions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Positions
                .Include(p => p.Order)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Positions", new { id = position.OrderId, isToday = true });
        }

        private bool PositionExists(int id)
        {
            return _context.Positions.Any(e => e.PositionId == id);
        }

      
    }
}
