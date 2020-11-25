using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Product
    {
        [Key]
        public long ProductId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Brand name")]
        public string Brand { get; set; }

        [Display(Name = "Country")]
        public virtual Country CreatedPlace  { get; set; }

        [Display(Name = "Price for one killogram")]
        public double PriceForOneKilogram { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Weight")]
        public float Weight { get; set; }

        [Display(Name = "Proteins")]
        public float Protein { get; set; }

        [Display(Name = "Fats")]
        public float Fat { get; set; }

        [Display(Name = "Carbohydrates")]
        public float Carbohydrates { get; set; }

        [Display(Name = "Energy value")]
        public int EnergyValue { get; set; }

        [Display(Name = "Is deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Is in stoke")]
        public bool InStock { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageName { get; set; }

        [Display(Name = "Category")]
        public virtual Category Category { get; set; }

        [Display(Name = "Added date")]
        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }

        [Display(Name = "Picture")]
        public string ProductPicture { get; set; }
    }
}
