using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISTPLR_one.Models
{
    public class Query
    {
        public int OwnerID { get; set; }
        public List<string> CashiersNames { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public DateTime OrderDate { get; set; }
        public int CashierId { get; set; }
        public string CashierName { get; set; }
        public int AvgPrice { get; set; }


        public int CoffeeShopId { get; set; }
        public string CoffeeShopName { get; set; }
        public List<string> CatNames { get; set; }

        public int ProductId { get; set; }
        public List<string> OwnerNames { get; set; }
        public List<string> OwnerSurnames { get; set; }
        public List<string> OwnerPhones { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Range(0, 999)]
        public int ProductPrice { get; set; }
        public List<int> OrdersId { get; set; }
        public List<DateTime> OrdersDate { get; set; }

        public List<string> CoffeeShopNames { get; set; }
        public List<string> CoffeeShopAddresses { get; set; }
        public List<string> CashierPhones { get; set; }
    }
}