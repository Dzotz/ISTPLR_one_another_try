using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace ISTPLR_one
{
    public partial class CoffeeShop
    {
        public CoffeeShop()
        {
            Cashiers = new HashSet<Cashier>();
            Orders = new HashSet<Order>();
        }

        public int CoffeeShopId { get; set; }
        [Display(Name = "Власник")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public int OwnerId { get; set; }
        [Display(Name = "Адрес кав'ярні")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [MaxLength(50)]
        public string Address { get; set; }
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Display(Name = "Власник")]

        public virtual Owner Owner { get; set; }
        public virtual ICollection<Cashier> Cashiers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
