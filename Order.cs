using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ISTPLR_one
{
    public partial class Order
    {
        public Order()
        {
            Positions = new HashSet<Position>();
        }
        [Display(Name = "ID")]
        public int OrderId { get; set; }
        [Display(Name = "Дата замовлення")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я касира")]
        public int CashierId { get; set; }
        [Display(Name = "Кав'ярня")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public int CoffeeShopId { get; set; }
        [Display(Name = "Постійний покупець")]
        public int? BonusId { get; set; }
        [Display(Name = "Постійний покупець")]
        public virtual Client Bonus { get; set; }
        [Display(Name = "Бариста")]
        public virtual Cashier Cashier { get; set; }
        [Display(Name = "Адрес кав'ярні")]
        public virtual CoffeeShop CoffeeShop { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
