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
    }
}
