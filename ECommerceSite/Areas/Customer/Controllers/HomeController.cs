using ECommerceSite.Models;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerceSite.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _unitOfWork.Category.GetAll().ToList();
            return View(categoryList);
        }

        public IActionResult Products(int categoriesid)
        {
            List<Product> product = _unitOfWork.Product.GetAll().Where(u => u.CategoryId == categoriesid).ToList();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult ProductDetails(int productId)
        {
            var data = _unitOfWork.Product.Get(u => u.Id == productId);
            if(data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
