
using E_Tender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Tender.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Tender.Repository
{
    public class SQLTenderRepository : ITenderRepository
    {
        private readonly ApplicationDbContext context;

        public SQLTenderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        Tender ITenderRepository.Add(Tender tender)
        {
            context.Tenders.Add(tender);
            context.SaveChanges();
            return tender;
        }

        Tender ITenderRepository.Delete(int Id)
        {
            Tender t = context.Tenders.Find(Id);
            if (t != null)
            {
                context.Tenders.Remove(t);
                context.SaveChanges();
            }
            return t;
        }

        IEnumerable<Tender> ITenderRepository.GetAllTender()
        {
            return context.Tenders;
        }

        IEnumerable<Tender> ITenderRepository.GetAvalTender()
        {
            List<Tender> tenders = this.context.Tenders.AsNoTracking()
                .Where(n => (n.status==true))
                .Select(n => n)
                .ToList();
            return tenders;
        }

        Tender ITenderRepository.GetTender(int Id)
        {
            return context.Tenders.Find(Id);
        }

        IEnumerable<Tender> ITenderRepository.GetTenderByCompany(ApplicationUser id)
        {
            List<Tender> tenders = this.context.Tenders.AsNoTracking()
                .Where(n => (n.company_id.Equals(id)))
                .OrderBy(n=>n.Ending_Date)
                .Select(n => n)
                .ToList();
            return tenders;
        }

        Tender ITenderRepository.GetTenderByTender(Tender t)
        {
            List<Tender> tenders = this.context.Tenders.AsNoTracking()
                .Where(n => (n.Tender_Id.Equals(t.Tender_Id)))
                .Select(n => n).ToList();               
            return tenders[0];
        }

        IEnumerable<Tender> ITenderRepository.GetTenderByUser(ApplicationUser id)
        {
            List<Tender> tenders = this.context.Tenders.AsNoTracking()
                .Where(n => (n.user_id.Equals(id)))
                .OrderBy(n => n.Ending_Date)
                .Select(n => n)          
                .ToList();
            return tenders;
        }

        Tender ITenderRepository.Update(Tender tenderChanges)
        {
            var t = context.Tenders.Attach(tenderChanges);
            t.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return tenderChanges;
        }
    }
}
