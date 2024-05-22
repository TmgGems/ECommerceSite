using ECommereceSiteData.Data;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ECommerceSite.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Category> data = _db.Categories.ToList();
            var checkData = data;
            return View(checkData);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(Category modeldata, IFormFile img)
        {

            if (img != null && img.Length > 0)
            {
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", img.FileName);
                using (var filestream = System.IO.File.Create(filepath))
                {
                    img.CopyTo(filestream);
                }
            }


            if (ModelState.IsValid)
            {
                modeldata.ImageUrl = "/Photos/" + img.FileName;
                _db.Add(modeldata);
                _db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }
            return View(modeldata);


        }

        [HttpGet]

        public IActionResult Edit(int Id)
        {
            var data = _db.Categories.Find(Id);
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

        public IActionResult Edit(Category modeldata, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if (img != null && img.Length > 0)
                {
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", img.FileName);
                    using (var filestream = System.IO.File.Create(filepath))
                    {
                        img.CopyTo(filestream);
                    }
                    modeldata.ImageUrl = "/Photos/" + img.FileName;
                }
                else
                {
                    // Retain the old image URL if no new image is uploaded
                    var existingCategory = _db.Categories.AsNoTracking().FirstOrDefault(c => c.Id == modeldata.Id);
                    if (existingCategory != null)
                    {
                        modeldata.ImageUrl = existingCategory.ImageUrl;
                    }
                }
                _db.Update(modeldata);
                _db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return View(modeldata);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _db.Categories.Find(id);

            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]

        public IActionResult Delete(Category modeldata)
        {
            //Category? obj = _db.Categories.Find(u => u.Id == id);

            //if (obj == null)
            //{
            //    return NotFound();
            //}

            //if(ModelState.IsValid)
            //{
            //    _db.Categories.Remove(obj);
            //    _db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            _db.Categories.Remove(modeldata);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }

}