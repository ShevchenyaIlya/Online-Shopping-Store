using ProductStore.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Comment
    {
        [Key]
        public long CommentId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string CommentTitle { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Body")]
        public string CommentBody { get; set; }

        [Required]
        [Display(Name = "User")]
        public ApplicationUser CommentUser { get; set; }

        [Required]
        [Display(Name = "Product")]
        public Product CommentProduct { get; set; }

        [Required]
        [Display(Name = "Post date")]
        [DataType(DataType.DateTime)]
        public DateTime PostDate { get; set; }
    }
}
