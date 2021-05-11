using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ISTPLR_one.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : Controller
    {
        private readonly ISTPContext _context;

        public ChartsController(ISTPContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData() 
        {
            var selectCoffeeShops = _context.CoffeeShops.Join(_context.Orders, c => c.CoffeeShopId, o => o.CoffeeShopId, (c, o) => new
            {
                CoffeeShopId = c.CoffeeShopId,
                Name = c.Name,
                OrdersKol = _context.Orders.Where(ord => ord.CoffeeShopId == c.CoffeeShopId && DateTime.Now.Month == ord.Date.Month).Count()
            }).Distinct();
            List<object> elements = new List<object>();
            elements.Add(new[] { " ав'€рн€", " ≥льк≥сть замовлень за останн≥й м≥с€ць" });
            foreach(var s in selectCoffeeShops)
            {
                elements.Add(new object[]
                {
                    s.Name, s.OrdersKol
                });
            }
            return new JsonResult(elements);
        }


        [HttpGet("CategoriesPerDay")]
        public JsonResult CategoriesPerDay()
        {
            var positionsInOrdersThisMonth = _context.Positions.Where(p => p.Order.Date.Month == DateTime.Now.Month).Include(p=>p.Order);
            var daysWithCategories = positionsInOrdersThisMonth.Join(_context.Products, pos => pos.ProductId, pro => pro.ProductId, (pos, pro) => new
            {
                Category = pro.CategoryId,
                Name = pro.Category.Name,
                Day = pos.Order.Date.Day,
                
            });
            List<string> categoryNames = new List<string>();
            categoryNames.Add("ƒень м≥с€ц€");
            categoryNames.AddRange(daysWithCategories.Select(d => d.Name).Distinct().ToList());
           
            List<object> elements = new List<object>();
            elements.Add(categoryNames.ToArray());
           
            for(int i = 1;i<=DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                List<int> elem = new List<int>();
                elem.Add(i);
                foreach(var name in daysWithCategories.Select(d => d.Name).Distinct().ToList())
                {
                    elem.Add(daysWithCategories.Where(d => d.Day == i && d.Name == name).Count());
                }
                elements.Add(elem.ToArray());
            }
            return new JsonResult(elements);
        }

    }
}
