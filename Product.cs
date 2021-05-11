using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace ISTPLR_one
{
    public partial class Product
    {
        public Product()
        {
            Clients = new HashSet<Client>();
            Positions = new HashSet<Position>();
        }

        public int ProductId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ціна")]
        [Range(0,999)]
        public int Price { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }
        [Display(Name = "Категорія")]
        public virtual Category Category { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
