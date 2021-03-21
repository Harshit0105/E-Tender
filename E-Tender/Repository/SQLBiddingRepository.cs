using E_Tender.Data;
using E_Tender.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Tender.Repository
{
    public class SQLBiddingRepository : IBiddingRepository
    {
        private readonly ApplicationDbContext context;

        public SQLBiddingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        Bidding IBiddingRepository.Add(Bidding bid)
        {
            context.Biddings.Add(bid);
            context.SaveChanges();
            return bid;
        }

        Bidding IBiddingRepository.Delete(int Id)
        {
            Bidding bid = context.Biddings.Find(Id);
            if (bid != null)
            {
                context.Biddings.Remove(bid);
                context.SaveChanges();
            }
            return bid;
        }

        IEnumerable<Bidding> IBiddingRepository.GetAllBids()
        {
            return context.Biddings;
        }

        Bidding IBiddingRepository.GetBidbyId(int Id)
        {
            return context.Biddings.Find(Id);
        }

        IEnumerable<Bidding> IBiddingRepository.GetBidsByCompany(ApplicationUser user)
        {
            List<Bidding> bids = this.context.Biddings.AsNoTracking()
                .Where(n => (n.company_id.Equals(user.Id.ToString())))                
                .Select(n => n)
                .ToList();
            return bids;
        }

        IEnumerable<Bidding> IBiddingRepository.GetBidsByTender(Tender tender)
        {
            List<Bidding> bids = this.context.Biddings.AsNoTracking()
                .Where(n => (n.tender_id.Equals(tender.Tender_Id)))
                .Select(n => n)
                .ToList();
            return bids;
        }

        bool IBiddingRepository.IsBidDoneByCompany(ApplicationUser user, Tender t)
        {
            List<Bidding> bids = this.context.Biddings.AsNoTracking()
                .Where(n => (n.tender_id.Equals(t) && n.company_id==user.Id.ToString()))                
                .Select(n => n)
                .ToList();
            if (bids.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        Bidding IBiddingRepository.Update(Bidding bidChanges)
        {
            var bid = context.Biddings.Attach(bidChanges);
            bid.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bidChanges;
        }
    }
}
