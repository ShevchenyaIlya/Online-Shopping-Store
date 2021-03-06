﻿using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.VIewModels
{
    public class CommentViewModel
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(200)]
        public string Body { get; set; }

        public Product Product { get; set; }
    }
}
