using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Tender.Models;
using E_Tender.Repository;
using E_Tender.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Tender.Controllers
{
    public class BidController : Controller
    {
        private readonly IBiddingRepository _bidRepo;
        private readonly ITenderRepository _tenderRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public BidController(SignInManager<ApplicationUser> signInManager, IBiddingRepository bidRepo, ITenderRepository tenderRepo, UserManager<ApplicationUser> userManager)
        {
            _bidRepo = bidRepo;
            _signInManager = signInManager;
            _userManager = userManager;
            _tenderRepo = tenderRepo;
        }
        public IActionResult Index()
        {
            var model = _bidRepo.GetAllBids();
            return View(model);
        }
        [HttpGet]
        public  ViewResult Add(int id)
        {
            BidViewModel bview = new BidViewModel();
            Tender t = _tenderRepo.GetTender(id);
            bview.tender =id;
            bview.tenderName = t.Tender_name;
            return View(bview);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BidViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                Console.WriteLine(user);
                Tender t = _tenderRepo.GetTender(model.tender);
                Bidding newBid = new Bidding
                {
                    amount=model.amount,
                    company_id=user.Id.ToString(),                    
                    tender_id=model.tender,                    
                    selected=false,
                };
                _bidRepo.Add(newBid);
                return RedirectToAction("index");
            }
            Console.WriteLine("Error!!");
            return View();
        }
    }
}
