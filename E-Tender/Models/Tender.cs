using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Tender.Models
{
    public class Tender
    {
        [Key]
        [Required]
        public int Tender_Id { get; set; }
        [Required]
        public string Tender_name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Base_price { get; set; }

        [Required]
        public bool status { get; set; }

        [Required]
        public bool assigned { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser user_id { get; set; }

        [Required]
        [ForeignKey("CompanyId")]
        public ApplicationUser company_id { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime Starting_Date { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime Ending_Date { get; set; }
    }
}
