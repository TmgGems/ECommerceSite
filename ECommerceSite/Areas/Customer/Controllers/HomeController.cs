using ECommerceSite.Models;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpGet]
        public IActionResult ProductDetails(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult ProductDetails(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartformdb = _unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId == shoppingCart.ApplicationUserId 
            && u.ProductId == shoppingCart.ProductId);


            if(cartformdb == null)
            {
                //Add To Cart
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                //Cart already exists
                cartformdb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartformdb);
            }
            TempData["success"] = "Cart Updated Successfully";
            
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
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
