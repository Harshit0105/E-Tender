using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using E_Tender.Models;

namespace E_Tender.ViewModel
{
    public class BidViewModel
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required(ErrorMessage = "Please provide valid amount")]        
        public int amount { get; set; }
        [Required]
        public int tender { get; set; }
        [Required]
        public String tenderName { get; set; }

    }
}
