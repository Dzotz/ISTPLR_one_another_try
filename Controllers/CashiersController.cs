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
    public class CashiersController : Controller
    {
        private readonly ISTPContext _context;

        public CashiersController(ISTPContext context)
        {
            _context = context;
        }

        // GET: Cashiers
        public async Task<IActionResult> Index()
        {
            var iSTPContext = _context.Cashiers.Include(c => c.CoffeeShop).Include(c => c.Orders);
            return View(await iSTPContext.ToListAsync());
        }

        // GET: Cashiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.CoffeeShop).Include(c=>c.Orders)
                .FirstOrDefaultAsync(m => m.CashierId == id);

            var PositionsWithValues = _context.Positions.Join(_context.Products, pos => pos.ProductId, pro => pro.ProductId, (pos, pro) => new
            {
                OrderId = pos.OrderId,
                ProductId = pro.ProductId,
                Value = pos.Quantity * pro.Price
            });
            var NeededOrders = _context.Orders.Where(o => o.CashierId == id && o.Date.Month == DateTime.Now.Month).Include(o => o.Bonus);
           
            var OrdersWithValues = NeededOrders.Join(PositionsWithValues, o => o.OrderId, p => p.OrderId, (o, p) => new
            {
                OrderId = o.OrderId,
                Value = PositionsWithValues.Where(p => p.OrderId == o.OrderId && p.ProductId != o.Bonus.FavoriteProductId).Sum(p => p.Value)
            }).ToList();
            int kolSmen = 0;
            for (int i = 1; i <= DateTime.Now.Day;i++)
            {
                if (_context.Orders.Where(o => o.CashierId == id && o.Date.Month == DateTime.Now.Month && o.Date.Day == i).Count() != 0)
                {
                    kolSmen++;
                }
            }
            ViewBag.Salary = OrdersWithValues.Sum(o => o.Value)*0.05 + 250*kolSmen;
            
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        // GET: Cashiers/Create
        public IActionResult Create()
        {
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name");
            return View();
        }

        // POST: Cashiers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CashierId,CoffeeShopId,Name,Surname,Fathername,PhoneNumber")] Cashier cashier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cashier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name", cashier.CoffeeShopId);
            return View(cashier);
        }

        // GET: Cashiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return NotFound();
            }
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name", cashier.CoffeeShopId);
            return View(cashier);
        }

        // POST: Cashiers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CashierId,CoffeeShopId,Name,Surname,Fathername,PhoneNumber")] Cashier cashier)
        {
            if (id != cashier.CashierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cashier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashierExists(cashier.CashierId))
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
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name", cashier.CoffeeShopId);
            return View(cashier);
        }

        // GET: Cashiers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.CoffeeShop)
                .FirstOrDefaultAsync(m => m.CashierId == id);
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        // POST: Cashiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cashier = await _context.Cashiers.FindAsync(id);
            _context.Cashiers.Remove(cashier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashierExists(int id)
        {
            return _context.Cashiers.Any(e => e.CashierId == id);
        }
    }
}
