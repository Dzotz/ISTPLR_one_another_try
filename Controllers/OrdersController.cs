using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISTPLR_one;
using ClosedXML.Excel;
using System.IO;

namespace ISTPLR_one.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ISTPContext _context;

        public OrdersController(ISTPContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int id, string date)
        {
            DateTime orderDate;
            DateTime.TryParse(date, out orderDate);
            ViewBag.isToday = orderDate.ToShortDateString() == DateTime.Now.ToShortDateString();
            var neededOrders = _context.Orders.Where(o => o.CoffeeShopId == id && o.Date == orderDate).Include(o=>o.Bonus);
            var iSTPContext = neededOrders.Include(o => o.Cashier).Include(o => o.CoffeeShop).Include(o => o.Positions);
            var positionsWithValues = _context.Positions.Join(_context.Products, pos => pos.ProductId, pro => pro.ProductId, (pos, pro) => new
            {
                OrderId = pos.OrderId,
                ProductId = pro.ProductId,
                Value = pos.Quantity * pro.Price
            });
            var OrdersWithValues = neededOrders.Join(positionsWithValues, o => o.OrderId, p => p.OrderId, (o, p) => new
            {
                OrderId = o.OrderId,
                Value = p.ProductId != o.Bonus.FavoriteProductId ? positionsWithValues.Where(p => p.OrderId == o.OrderId).Sum(p => p.Value) : positionsWithValues.Where(p => p.OrderId == o.OrderId).Sum(p => p.Value) - _context.Products.Where(pro => pro.ProductId == p.ProductId).Select(pro => pro.Price).First()
                //Value = PositionsWithValues.Where(p => p.OrderId == o.OrderId && p.ProductId != o.Bonus.FavoriteProductId).Sum(p => p.Value)
            }).ToList();
            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach(var it in OrdersWithValues)
            {
                dic[it.OrderId] = it.Value;
            }
            ViewData["OrdersWithValues"] = dic;
            ViewBag.OrderCount = iSTPContext.Count();
            ViewBag.OrderDate = date;
            ViewBag.CoffeeShopId = id;
            ViewBag.CoffeeShopName = _context.CoffeeShops.Where(c => c.CoffeeShopId == id).Select(c => c.Name).FirstOrDefault();
            return View(await iSTPContext.ToListAsync());
        }

        

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id, bool isToday)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Bonus)
                .Include(o => o.Cashier)
                .Include(o => o.CoffeeShop)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Positions", new { id = order.OrderId, isToday=isToday });
        }

       
        // GET: Orders/Create
        public IActionResult Create(int id, string date)
        {
            DateTime orderDate;
            DateTime.TryParse(date, out orderDate);
            ViewData["BonusId"] = new SelectList(_context.Clients, "ClientId", "Name");
            ViewBag.OrderDate = orderDate;
            ViewData["CoffeeShopId"] = id;
            ViewData["CashierId"] = new SelectList(_context.Cashiers.Where(b=>b.CoffeeShopId==id), "CashierId", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Date,CashierId,CoffeeShopId,BonusId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = order.CoffeeShopId, date = order.Date.ToShortDateString() });
            }
            ViewData["BonusId"] = new SelectList(_context.Clients, "ClientId", "Name", order.BonusId);
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "CashierId", "Fathername", order.CashierId);
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name");
            ViewData["CoffeeShopId2"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name", order.CoffeeShopId);

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["BonusId"] = new SelectList(_context.Clients, "ClientId", "Name", order.BonusId);
            ViewData["CashierId"] = new SelectList(_context.Cashiers.Where(b => b.CoffeeShopId == order.CoffeeShopId), "CashierId", "Name", order.CashierId);
            ViewBag.CoffeeShopId = order.CoffeeShopId;
            ViewBag.OrderDate = order.Date;
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Date,CashierId,CoffeeShopId,BonusId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = order.CoffeeShopId, date = order.Date.ToShortDateString() });
            }
            ViewData["BonusId"] = new SelectList(_context.Clients, "ClientId", "Name", order.BonusId);
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "CashierId", "Name", order.CashierId);
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name", order.CoffeeShopId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Bonus)
                .Include(o => o.Cashier)
                .Include(o => o.CoffeeShop)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ViewReport( )
        {
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name");
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            if (month.Length == 1) month = "0" + month;
            string day = DateTime.Now.Day.ToString();
            if (day.Length == 1) day = "0" + day;
            string x = year + "-" + month + "-" + day;
            ViewBag.Today = x;
            return View();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }


        public ActionResult Export(int id, string date, int total)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                DateTime orderDate;
                DateTime.TryParse(date, out orderDate);

                var neededOrders = _context.Orders.Where(o => o.CoffeeShopId == id && o.Date == orderDate).Include(o => o.Bonus).Include(o => o.Cashier).Include(o => o.CoffeeShop).Include(o => o.Positions);
                var positionsWithValues = _context.Positions.Join(_context.Products.Include(p=>p.Category), pos => pos.ProductId, pro => pro.ProductId, (pos, pro) => new
                {
                    OrderId = pos.OrderId,
                    ProductId = pro.ProductId,
                    Name = pro.Name,
                    Category = pro.Category.Name,
                    Quantity = pos.Quantity,
                    Value = pos.Quantity * pro.Price,
                    Description = pro.Description
                }) ;
                var ordersWithValues = neededOrders.Join(positionsWithValues, o => o.OrderId, p => p.OrderId, (o, p) => new
                {
                    OrderId = o.OrderId,
                    Value =  p.ProductId != o.Bonus.FavoriteProductId ? positionsWithValues.Where(p => p.OrderId == o.OrderId).Sum(p => p.Value) : positionsWithValues.Where(p => p.OrderId == o.OrderId).Sum(p => p.Value)-_context.Products.Where(pro => pro.ProductId == p.ProductId).Select(pro => pro.Price).First()
                }).ToList();
                var resWorksheet = workbook.Worksheets.Add("Результати");
                resWorksheet.Cell("A1").Value = "Номер замовлення";
                resWorksheet.Cell("B1").Value = "Бариста";
                resWorksheet.Cell("C1").Value = "Постійний покупець";
                resWorksheet.Cell("D1").Value = "Сплачено";
                var neededOrdersList = neededOrders.ToList();
                int j=0;
                for (j=0; j < neededOrders.Count(); j++)
                {
                    resWorksheet.Cell(j + 2, 1).Value = neededOrdersList[j].OrderId;
                    resWorksheet.Cell(j + 2, 2).Value = neededOrdersList[j].Cashier.Name;
                    resWorksheet.Cell(j + 2, 3).Value = neededOrdersList[j].Bonus.Name;
                    resWorksheet.Cell(j + 2, 4).Value = ordersWithValues.Where(o=>o.OrderId == neededOrdersList[j].OrderId).Select(o=>o.Value).FirstOrDefault();

                }
                resWorksheet.Cell(j + 3, 1).Value = "Загалом";
                resWorksheet.Cell(j + 3, 2).Value = total;
                resWorksheet.Row(j+3).Style.Font.Bold = true;
                resWorksheet.Row(1).Style.Font.Bold = true;

                foreach (var o in neededOrders)
                {
                    var worksheet = workbook.Worksheets.Add(o.OrderId.ToString());

                    worksheet.Cell("A1").Value = "Продукт";
                    worksheet.Cell("B1").Value = "Кількість";
                    worksheet.Cell("C1").Value = "Категорія";
                    worksheet.Cell("D1").Value = "Інформація";
                    worksheet.Cell("E1").Value = "Ціна";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var positionsWithValuesList = positionsWithValues.Where(p=>p.OrderId == o.OrderId).ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < positionsWithValuesList.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = positionsWithValuesList[i].Name;
                        worksheet.Cell(i + 2, 2).Value = positionsWithValuesList[i].Quantity;
                        worksheet.Cell(i + 2, 3).Value = positionsWithValuesList[i].Category;
                        worksheet.Cell(i + 2, 4).Value = positionsWithValuesList[i].Description;
                        if (o.Bonus.FavoriteProductId != positionsWithValuesList[i].ProductId)
                        {
                            worksheet.Cell(i + 2, 5).Value = positionsWithValuesList[i].Value;
                        }
                        else
                        {
                            worksheet.Cell(i + 2, 5).Value = positionsWithValuesList[i].Value -_context.Products.Where(p=>p.ProductId==positionsWithValuesList[i].ProductId).Select(p=>p.Price).First();
                            if(Convert.ToInt32(worksheet.Cell(i + 2, 5).Value) == 0)
                            {
                                worksheet.Cell(i + 2, 5).Value = "Знижка!";
                            }
                        }


                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Report_{_context.CoffeeShops.Where(c=>c.CoffeeShopId == id).Select(c=>c.Name).FirstOrDefault()}_{date}.xlsx"
                    };
                }
            }
        }

    }
}
