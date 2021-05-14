using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISTPLR_one;
using ISTPLR_one.Models;
using Microsoft.Data.SqlClient;



namespace ISTPLR_one.Controllers
{
    public class QueryController : Controller
    {
        private readonly ISTPContext _context;
        private const string CONN_STR = "Server=DESKTOP-OJTBMVP; Database=ISTP; Trusted_Connection=True;MultipleActiveResultSets=true";
        private const string S1 = @"Queries\S1.sql";
        private const string S2 = @"Queries\S2.sql";
        private const string S3 = @"Queries\S3.sql";
        private const string S4 = @"Queries\S4.sql";
        private const string S5 = @"Queries\S5.sql";
        private const string C1 = @"Queries\C1.sql";
        private const string C2 = @"Queries\C2.sql";
        private const string C3 = @"Queries\C3.sql";

        public QueryController(ISTPContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["CoffeeShopId"] = new SelectList(_context.CoffeeShops, "CoffeeShopId", "Name");
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "CashierId", "Name");
            ViewData["Name"] = new SelectList(_context.Cashiers, "Name", "Name");
            ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerId", "Name"); ;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S1);
            query = query.Replace("P", "N\'" + queryModel.OwnerID + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.CashiersNames = new List<string>();
            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryModel.CashiersNames.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return View(queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S2);
            queryModel.CashierName = _context.Cashiers.Where(c => c.CashierId == queryModel.CashierId).Select(c => c.Name).FirstOrDefault();
            query = query.Replace("X", "N\'" + queryModel.OrderDate.ToShortDateString() + "\'");
            query = query.Replace("Y", "N\'" + queryModel.CashierId + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        queryModel.AvgPrice = Convert.ToInt32(result);
                    }

                }
            }
            return View(queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S3);
            queryModel.CoffeeShopName = _context.CoffeeShops.Where(c => c.CoffeeShopId == queryModel.CoffeeShopId).Select(c => c.Name).FirstOrDefault();
            query = query.Replace("X", "N\'" + queryModel.CoffeeShopId + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.CatNames = new List<string>();
            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryModel.CatNames.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return View(queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery4(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S4);
            query = query.Replace("X", "N\'" + queryModel.ProductId + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.OwnerNames = new List<string>();
            queryModel.OwnerSurnames = new List<string>();
            queryModel.OwnerPhones = new List<string>();
            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryModel.OwnerNames.Add(reader.GetString(0));
                            queryModel.OwnerSurnames.Add(reader.GetString(1));
                            queryModel.OwnerPhones.Add(reader.GetString(2));
                        }
                    }
                }
                connection.Close();
            }
            return View(queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery5(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S5);
            query = query.Replace("X", "N\'" + queryModel.ProductPrice + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.OrdersId = new List<int>();
            queryModel.OrdersDate = new List<DateTime>();
            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryModel.OrdersId.Add(reader.GetInt32(0));
                            queryModel.OrdersDate.Add(reader.GetDateTime(1));
                        }
                    }
                }
                connection.Close();
            }
            return View(queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ComplexQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(C1);
            query = query.Replace("X", "N\'" + queryModel.OwnerID + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.CoffeeShopNames = new List<string>();
            queryModel.CoffeeShopAddresses = new List<string>();
            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryModel.CoffeeShopNames.Add(reader.GetString(0));
                            queryModel.CoffeeShopAddresses.Add(reader.GetString(1));
                        }
                    }
                }
                connection.Close();
            }
            return View(queryModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ComplexQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(C2);
            query = query.Replace("X", "N\'" + queryModel.OrderDate + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.OrdersId = new List<int>();
            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryModel.OrdersId.Add(reader.GetInt32(0));
                        }
                    }
                }
                connection.Close();
            }
            return View(queryModel);
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ComplexQuery3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(C3);
            query = query.Replace("X", "N\'" + queryModel.CashierName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.CashierPhones = new List<string>();
            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            queryModel.CashierPhones.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return View(queryModel);
        }
    }
}
   
