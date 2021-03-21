using E_Tender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Tender.Repository
{
    public interface IBiddingRepository
    {
        Bidding GetBidbyId(int Id);
        IEnumerable<Bidding> GetBidsByTender(Tender tender);
        IEnumerable<Bidding> GetBidsByCompany(ApplicationUser user);
        IEnumerable<Bidding> GetAllBids();
        Bidding Add(Bidding bid);
        Bidding Update(Bidding bidChanges);
        Bidding Delete(int Id);
        Boolean IsBidDoneByCompany(ApplicationUser user,Tender t);
    }
}
