using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Tender.NewFolder
{
    public class TenderViewModel
    {

        [Key]
        [Required]
        public int id { get; set; }
        [Required(ErrorMessage = "Please provide some description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please provide some name")]
        [Display(Name = "Name")]
        public string Tender_name { get; set; }
        [Required(ErrorMessage = "Please provide base price")]
        [Display(Name = "Base Price")]        
        public int Base_price { get; set; }
   
        [Required]
        [Display(Name = "Tender is open")]
        public bool status { get; set; }

        [Required]
        [Display(Name = "Tender is Assigned")]
        public bool assigned { get; set; }


        [Required(ErrorMessage = "Please Select starting Date for tender")]
        [Display(Name = "Start Date")]
        public DateTime Starting_Date { get; set; }

        [Required(ErrorMessage = "Please Select End Date to close tender")]
        [Display(Name = "End Date")]
        public DateTime Ending_Date { get; set; }
    }
}
