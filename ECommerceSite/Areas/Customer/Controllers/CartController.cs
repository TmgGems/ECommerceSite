using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using ECommereceSiteModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceSite.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product")
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Count * cart.Price);

            }
            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            return View();
        }

        private double GetPriceBasedQuantity(ShoppingCart shoppingCart)
        {
            double productPrice = double.Parse(shoppingCart.Product.discountRate);

            //if (shoppingCart.Count > 3)
            //{
            //    return productPrice - 500;
            //}
            //else
            //{
            //    if(shoppingCart.Count > 5)
            //    {
            //        return productPrice - 1500;
            //    }
            //    else
            //    {
            return productPrice;
            //    }
            //}
        }


        
        public IActionResult plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if(cartFromDb.Count <= 1)
            {
                //remove the cart
                _unitOfWork.ShoppingCart.Remove(cartFromDb);

            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}

