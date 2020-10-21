using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.VIewModels
{
    public class RatingViewModel
    {
        public double TotalRating { get; set; }
        public int FullStars { get; set; }
        public bool HalfStar { get; set; }
        public int EmptyStart { get; set; }

        public int MarkCount { get; set; }
    }
}
