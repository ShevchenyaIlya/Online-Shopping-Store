using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Models
{
    public class MailRequest
    {
        [Display(Name = "Send to")]
        public string ToEmail { get; set; }
        [Display(Name = "Subject:")]
        public string Subject { get; set; }
        [Display(Name = "Body")]
        public string Body { get; set; }
        [Display(Name = "Attachments")]
        public List<IFormFile> Attachments { get; set; }
    }
}
