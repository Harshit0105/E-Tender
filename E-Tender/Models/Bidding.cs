using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata;

namespace E_Tender.Models
{
    public class Bidding
    {
        [Key]
        [Required]
        public int Bid_Id { get; set; }

        [Required]
        public int amount { get; set; }

        [Required]
        public bool selected { get; set; }

        [Required]        
        public int tender_id { get; set; }               
        [Required]       
        public string company_id { get; set; }        
    }
}
