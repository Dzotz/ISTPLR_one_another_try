using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


#nullable disable

namespace ISTPLR_one
{
    public partial class Owner
    {
        public Owner()
        {
            CoffeeShops = new HashSet<CoffeeShop>();
        }

        public int OwnerId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [MaxLength(50)]
        [Display(Name = "Ім'я власника")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [MaxLength(50)]
        [Display(Name = "Фамілія")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [MaxLength(50)]
        [Display(Name = "По батькові")]
        public string Fathername { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [RegularExpression(@"^\+[3][8]\([0-9]{3}\)[0-9]{3}\-[0-9]{2}\-[0-9]{2}$", ErrorMessage = "Phone Number in form +38(xxx)xxx-xx-xx")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }

        public virtual ICollection<CoffeeShop> CoffeeShops { get; set; }
    }
}
