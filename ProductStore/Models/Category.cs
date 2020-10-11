using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        public string CategoryDescription { get; set; }

        [Display(Name = "Is deleted")]
        public bool IsDeleted { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Display(Name = "Picture")]
        public byte[] CategoryPicture { get; set; }

        [Display(Name = "Added date")]
        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; }
    }
}
