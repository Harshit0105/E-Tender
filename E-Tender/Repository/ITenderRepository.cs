using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Tender.Models;

namespace E_Tender.Repository
{
    public interface ITenderRepository
    {
        Tender GetTender(int Id);
        Tender GetTenderByTender(Tender t);
        IEnumerable<Tender> GetTenderByUser(ApplicationUser user);
        IEnumerable<Tender> GetTenderByCompany(ApplicationUser user);
        IEnumerable<Tender> GetAllTender();
        IEnumerable<Tender> GetAvalTender();
        Tender Add(Tender tender);
        Tender Update(Tender tenderChanges);
        Tender Delete(int Id);
    }
}
