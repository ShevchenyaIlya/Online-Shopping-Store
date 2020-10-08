using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Display(Name = "Product")]
        public Product Product { get; set; }

        [Required]
        [Display(Name = "Sale percent")]
        public float SaleValue { get; set; }

        [Display(Name = "Creation sale date")]
        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }

        [Display(Name = "Start sale date")]
        [DataType(DataType.DateTime)]
        public DateTime FromDate { get; set; }

        [Display(Name = "Finish sale date")]
        [DataType(DataType.DateTime)]
        public DateTime ToDate { get; set; }
    }
}
