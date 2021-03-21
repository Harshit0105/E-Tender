using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Tender.ViewModel
{
    public class BidListViewModel
    {
        public int id { get; set; }
        public int tenderId { get; set; }
        public string tenderName { get; set; }
        public string companyName { get; set; }
        public int amount { get; set; }
        public bool status { get; set; }
    }
}
