using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [Display(Name = "Product")]
        public Product Product { get; set; }

        [Display(Name = "User marks")]
        public List<Mark> UserMarks { get; set; }
    }
}
