using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Country name")]
        public string CountryName { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Country abbreviation")]
        public string CountryAbbreviation { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Country code")]
        public string CountryCode { get; set; }

        [Display(Name = "Country image")]
        public byte[] CountryPicture { get; set; }
    }
}
