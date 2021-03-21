using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Tender.Models;
using E_Tender.Repository;
using E_Tender.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace E_Tender.Controllers
{
    [Authorize(Roles ="Company")]
    public class CompanyController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITenderRepository _tenderRepo;
        private readonly IBiddingRepository _bidRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyController(ITenderRepository tenderRepo, IBiddingRepository bidRepo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _tenderRepo = tenderRepo;
            _bidRepo = bidRepo;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Tender()
        {
            IEnumerable<Tender> model = _tenderRepo.GetAvalTender();
            var user = await _userManager.GetUserAsync(User);
            var temp = model.ToList();
            foreach(Tender t in model)
            {
                if (_bidRepo.IsBidDoneByCompany(user, t))
                {
                    temp.Remove(t);
                }
            }
            model = (IEnumerable<Tender>)temp;
            return View(model);
        }
        public async Task<IActionResult> AssignedTender()
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<Tender> model = _tenderRepo.GetTenderByCompany(user);                                    
            return View(model);
        }
        public async Task<IActionResult> Bids()
        {
            var user = await _userManager.GetUserAsync(User);
            var userID = user.Id.ToString();
            IEnumerable<Bidding> model = _bidRepo.GetBidsByCompany(user);
            List<BidListViewModel> bidList = new List<BidListViewModel>();
            foreach(Bidding b in model)
            {                
                BidListViewModel bl = new BidListViewModel();
                bl.amount = b.amount;
                bl.companyName = user.UserName;
                Tender t = _tenderRepo.GetTender(b.tender_id);
                bl.tenderName = t.Tender_name;
                bl.status = t.status;
                bl.tenderId = t.Tender_Id;
                bidList.Add(bl);

            }
            IEnumerable<BidListViewModel> model1 = bidList;
            return View(model1);
        }
        [HttpGet]
        public ViewResult BidInTender(int id)
        {
            BidViewModel bview = new BidViewModel();
            Tender t = _tenderRepo.GetTender(id);
            bview.tender = id;
            bview.tenderName = t.Tender_name;
            return View(bview);
        }

        [HttpPost]
        public async Task<IActionResult> BidInTender(BidViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                Console.WriteLine(user);
                Tender t = _tenderRepo.GetTender(model.tender);               
                Bidding newBid = new Bidding
                {
                    amount = model.amount,
                    company_id = user.Id.ToString(),
                    tender_id = t.Tender_Id,
                    selected = false,
                };
                _bidRepo.Add(newBid);
                return RedirectToAction("index");
                             
            }
            Console.WriteLine("Error!!");
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Bidding b = _bidRepo.GetBidbyId(id);
            if (b == null)
            {
                return RedirectToAction("Bids");
            }
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteBus(int id)
        {
            Console.WriteLine(id);
            var t = _bidRepo.GetBidbyId(id);
            if (t != null)
                _bidRepo.Delete(t.Bid_Id);
            return RedirectToAction("Bids");
        }
    }
}
