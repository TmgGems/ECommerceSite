using ECommerceSite.Utility;
using ECommereceSiteData.Data;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class CompanyController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _db;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            var data = _unitOfWork.Company.GetAll().ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company modeldata)
        {
            var data = _unitOfWork.Company.Get(u => u.Name == modeldata.Name);
            if (data != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {

                _unitOfWork.Company.Add(modeldata);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Company");
            }
            return View(modeldata);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            

            var data = _unitOfWork.Company.Get(u => u.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return View(data);
            }
        }

        [HttpPost]
        public IActionResult Edit(Company modeldata)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Update(modeldata);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Company");
            }
            else
            {
                return View(modeldata);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _unitOfWork.Company.Get(x => x.Id == id);
            if (data != null)
            {
                return View(data);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            Company? obj = _unitOfWork.Company.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Remove(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(obj);
        }
    }
}
