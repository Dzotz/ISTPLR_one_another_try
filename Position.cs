using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace ISTPLR_one
{
    public partial class Position
    {
        public int PositionId { get; set; }
        public int ProductId { get; set; }
        [Display(Name = "Кількість")]
        [Range(1, 999)]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        public int OrderId { get; set; }
        [Display(Name = "Замовлення")]
        public virtual Order Order { get; set; }
        [Display(Name = "Продукт")]
        public virtual Product Product { get; set; }
    }
}
