using ProductStore.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Mark
    {
        [Key]
        public int MarkId { get; set; }

        [Display(Name = "Product")]
        public Product Product { get; set; }

        [Display(Name = "User")]
        public ApplicationUser User { get; set; }

        [Display(Name = "Whole mark")]
        public double TotalMark { get; set; }
    }
}
