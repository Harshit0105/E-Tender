using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_Tender.Repository;
using E_Tender.Models;
using Microsoft.AspNetCore.Identity;
using E_Tender;
using E_Tender.NewFolder;

namespace E_Tender.Controllers
{
    public class TendersController : Controller
    {
        private readonly ITenderRepository _tenderRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public TendersController(ITenderRepository tenderRepo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _tenderRepo = tenderRepo;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = _tenderRepo.GetAllTender();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            Tender tender = _tenderRepo.GetTender(id);
            if (tender == null)
                return RedirectToAction("index");
            return View(tender);
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Tender t = _tenderRepo.GetTender(id);
            TenderViewModel newTender = new TenderViewModel
            {
                id=t.Tender_Id,               
                Base_price=t.Base_price,
                Description=t.Description,                
                Ending_Date=t.Ending_Date,
                Starting_Date=t.Starting_Date,
                status=t.status,
            };
            return View(newTender);
        }

        [HttpPost]
        public IActionResult Edit(TenderViewModel model)
        {
            if (ModelState.IsValid)
            {
                Tender tender = _tenderRepo.GetTender(model.id);
                tender.Starting_Date = model.Starting_Date;
                tender.Ending_Date = model.Ending_Date;
                tender.status = model.status;               
                tender.Description = model.Description;
                tender.Base_price = model.Base_price;                     
                Tender updatedTender = _tenderRepo.Update(tender);
                return RedirectToAction("index");
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
                    Base_price = model.Base_price,
                    Description = model.Description,
                    company_id = user,
                    Ending_Date = model.Ending_Date,
                    Starting_Date = model.Starting_Date,
                    status = model.status,
                    assigned=false,
                };
                _tenderRepo.Add(newTender);                
                return RedirectToAction("index");
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
                return RedirectToAction("index");
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
            return RedirectToAction("index");
        }
    }
}
