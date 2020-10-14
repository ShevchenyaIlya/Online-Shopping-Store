using ProductStore.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Order
    {
        [Key]
        public long OrderId { get; set; }

        [Display(Name = "Addres")]
        public string Addres { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }

        [Display(Name = "Paid amount")]
        public double AmountPaid { get; set; }

        [Display(Name = "Payment type")]
        public string PaymentType { get; set; }
        public virtual ApplicationUser Customer { get; set; }

        [Display(Name = "Formed order date")]
        [DataType(DataType.DateTime)]
        public DateTime FormedDate { get; set; }
    }
}
