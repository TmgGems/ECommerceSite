using ECommereceSiteData.Data;using ECommereceSiteModels.Models;using Microsoft.AspNetCore.Mvc;using System.IO;namespace ECommerceSite.Controllers{    public class CategoryController : Controller    {        private readonly IWebHostEnvironment _webHostEnvironment;        private readonly ApplicationDbContext _db;        public CategoryController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)        {            _db = db;            _webHostEnvironment = webHostEnvironment;        }        public IActionResult Index()        {            List<Category> data = _db.Categories.ToList();
            var checkData = data;
            return View(checkData);        }        public IActionResult Create()        {            return View();        }        [HttpPost]        public  IActionResult Create(Category modeldata, IFormFile img)        {            
            if(img != null && img.Length > 0)
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
                _db.Add(modeldata);s
                _db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }
            return View(modeldata);

            //string imageUrl = null;            //if (img != null && img.Length > 0)            //{            //    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Photos");            //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(img.FileName);            //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);            //    using (var fileStream = new FileStream(filePath, FileMode.Create))            //    {            //        img.CopyToAsync(fileStream);            //    }            //    imageUrl = Path.Combine("/Photos", uniqueFileName);            //}

            //return View(modeldata);
            //if (ModelState.IsValid)
            //{
            //    var data = new Category
            //    {
            //        Name = modeldata.Name,
            //        ImageUrl = imageUrl
            //    };

            //    _db.Add(data);
            //    _db.SaveChanges();
            //}
            //return RedirectToAction("Index", "Category");

        }
    }}