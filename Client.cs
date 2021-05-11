using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ISTPLR_one
{
    public partial class Client
    {
        public Client()
        {
            Orders = new HashSet<Order>();
        }

        public int ClientId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Номер телефону")]
        [RegularExpression(@"^\+[3][8]\([0-9]{3}\)[0-9]{3}\-[0-9]{2}\-[0-9]{2}$", ErrorMessage = "Phone Number in form +38(xxx)xxx-xx-xx")]

        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Улюблений продукт")]

        public int FavoriteProductId { get; set; }
        [Display(Name = "Улюблений продукт")]

        public virtual Product FavoriteProduct { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
