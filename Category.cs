using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ISTPLR_one
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [MaxLength(50)]
        [Display(Name = "Назва категорії")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
