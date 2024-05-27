using ECommereceSiteData.Data;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceSite.Controllers
{
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            var data = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()

            });
            ViewBag.CategoryList = CategoryList;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product modeldata, IFormFile ProductImg)
        {
            var data = _unitOfWork.Product.Get(u => u.ProductName == modeldata.ProductName);
            if (data != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                if (ProductImg != null && ProductImg.Length > 0)
                {
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", ProductImg.FileName);
                    using (var filestream = System.IO.File.Create(filepath))
                    {
                        ProductImg.CopyTo(filestream);
                    }
                    modeldata.ImageUrl = "/Photos/" + ProductImg.FileName;
                }


                if (ModelState.IsValid)
                {

                    _unitOfWork.Product.Add(modeldata);
                    _unitOfWork.Save();
                    return RedirectToAction("Index", "Product");
                }
                return View(modeldata);

            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()

            });
            ViewBag.CategoryList = CategoryList;

            var data = _unitOfWork.Product.Get(u => u.Id == id);
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
        public IActionResult Edit(Product modeldata, IFormFile ? img)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                if (img != null && img.Length > 0)
                {
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", img.FileName);

                    if (!string.IsNullOrEmpty(modeldata.ImageUrl))
                    {
                        //Delete old image
                        try
                        {
                            // Construct the full old image path
                            var oldImagePath = Path.Combine(wwwRootPath, modeldata.ImageUrl.TrimStart('/'));

                            // Delete old image if it exists
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception or handle it as needed
                            Console.WriteLine($"Error deleting old image: {ex.Message}");
                        }

                    }
                    using (var filestream = System.IO.File.Create(filepath))
                    {
                        img.CopyTo(filestream);
                    }
                    modeldata.ImageUrl = "/Photos/" + img.FileName;
                }
                else
                {
                    // Retain the old image URL if no new image is uploaded
                    var existingProduct = _unitOfWork.Product.Get(c => c.Id == modeldata.Id);
                    if (existingProduct != null)
                    {
                        modeldata.ImageUrl = existingProduct.ImageUrl;
                    }
                }
                _unitOfWork.Product.Update(modeldata);
                _unitOfWork.Save();
                return RedirectToAction("Index", "Product");
            }
            else
            {
                return View(modeldata);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _unitOfWork.Product.Get(x => x.Id == id);
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
        public IActionResult Delete(int ? id) 
        {
            Product ? obj = _unitOfWork.Product.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Remove(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(obj);
        }
    }
}
