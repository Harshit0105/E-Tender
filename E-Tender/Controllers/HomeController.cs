using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using E_Tender.Models;
using E_Tender.Repository;
using Microsoft.AspNetCore.Identity;
using E_Tender.NewFolder;
using Microsoft.AspNetCore.Authorization;

namespace E_Tender.Controllers
{
    [Authorize(Roles ="User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITenderRepository _tenderRepo;
        private readonly IBiddingRepository _bidRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ITenderRepository tenderRepo, IBiddingRepository bidRepo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,ILogger<HomeController> logger)
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
        public IActionResult Details(int id)
        {
            Tender tender = _tenderRepo.GetTender(id);
            if (tender == null)
                return RedirectToAction("index");
            return View(tender);
        }
        public async Task<IActionResult> Tender()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _tenderRepo.GetTenderByUser(user);
            return View(model);
        }

        public async Task<IActionResult> CloseTender(int id)
        {
            Tender t = _tenderRepo.GetTender(id);
            IEnumerable<Bidding> bids = _bidRepo.GetBidsByTender(t);
            int min = int.MaxValue;
            String indexId = "";
            bool find = false;
            foreach(Bidding b in bids)
            {
                if((b.amount < min) && b.amount>=t.Base_price)
                {
                    min = b.amount;
                    indexId = b.company_id;
                    find = true;
                }
            }
            if (find)
            {
                var user = await _userManager.FindByIdAsync(indexId);
                t.Starting_Date = t.Starting_Date;
                t.Tender_name = t.Tender_name;
                t.Ending_Date = t.Ending_Date;
                t.status = false;
                t.assigned = true;
                t.Description = t.Description;
                t.Base_price = t.Base_price;
                t.company_id = user;
                Tender updatedTender = _tenderRepo.Update(t);
            }
            return RedirectToAction("Tender");
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Tender t = _tenderRepo.GetTender(id);
            TenderViewModel newTender = new TenderViewModel
            {
                id = t.Tender_Id,
                Tender_name=t.Tender_name,
                Base_price = t.Base_price,
                Description = t.Description,
                Ending_Date = t.Ending_Date,
                Starting_Date = t.Starting_Date,
                status = t.status,
            };
            return View(newTender);
        }

        [HttpGet]
        public ViewResult Bidding(int id)
        {           
            return View();
        }

        [HttpPost]
        public IActionResult Edit(TenderViewModel model)
        {
            if (ModelState.IsValid)
            {
                Tender tender = _tenderRepo.GetTender(model.id);
                tender.Starting_Date = model.Starting_Date;
                tender.Tender_name = model.Tender_name;
                tender.Ending_Date = model.Ending_Date;
                tender.status = model.status;
                tender.Description = model.Description;
                tender.Base_price = model.Base_price;
                Tender updatedTender = _tenderRepo.Update(tender);
                return RedirectToAction("Tender");
            }
            return View(model);
        }
        [HttpGet]
        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(TenderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                Console.WriteLine(user);
                Tender newTender = new Tender
                {
                    user_id = user,
                    Tender_name=model.Tender_name,
                    Base_price = model.Base_price,
                    Description = model.Description,
                    company_id = user,
                    Ending_Date = model.Ending_Date,
                    Starting_Date = model.Starting_Date,
                    status = model.status,
                    assigned = false,
                };
                _tenderRepo.Add(newTender);
                return RedirectToAction("Tender");
            }
            Console.WriteLine("Error!!");
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Tender t = _tenderRepo.GetTender(id);
            if (t == null)
            {
                return RedirectToAction("Tender");
            }
            return View(t);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteBus(int id)
        {
            Console.WriteLine(id);
            var t = _tenderRepo.GetTender(id);
            if (t != null)
                _tenderRepo.Delete(t.Tender_Id);
            return RedirectToAction("Tender");
        }
    }
}
