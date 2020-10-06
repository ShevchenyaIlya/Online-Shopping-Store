using ProductStore.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class ProductBasket
    {

        [Key]
        public long BasketId { get; set; }

        [Display(Name = "Customer")]
        public ApplicationUser Customer { get; set; }

        [Display(Name = "Products")]
        public List<Product> Products { get; set; }
    }
}
