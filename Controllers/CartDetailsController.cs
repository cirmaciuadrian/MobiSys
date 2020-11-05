using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MobiSys.Models;

namespace MobiSys.Controllers
{
    [Authorize(Roles ="Customer")]
    public class CartDetailsController : Controller
    {
        private readonly MobiSysContext _context;

        public CartDetailsController(MobiSysContext context)
        {
            _context = context;
        }


        
        public async Task<IActionResult> Index()
        {
            var user = await GetCustomerByUserIdAsync();
            ViewBag.credit = user.Credit;
            var cart = await GetCurrentUserCartAsync();
            var cartDetails = _context.CartDetails
               .Where(c => c.CartId == cart.Id)
               .Include(c => c.Cart)
               .Include(c => c.Product);
            await CalculaTetoatalAsync();
            return View(await cartDetails.ToListAsync());
        }




        public async Task<IActionResult> UseVaucherAsync(string vaucherInput)
        {



            var cart = await GetCurrentUserCartAsync();
            if (cart == null)
            {
                return NotFound();
            }
            var customer = await GetCustomerByUserIdAsync();
            if (customer == null)
            {
                return NotFound();
            }
            var vaucher = await _context.Vauchers.FirstOrDefaultAsync(c => c.CustomerId == customer.Id
                                                                && c.IsUsed == false
                                                                && c.Code == vaucherInput);
            if (vaucher == null)
            {

                TempData["error"] = "Vaucher Invalid";
                return RedirectToAction("Index");
            }


            if (vaucher.Value != null)
            {
                if (cart.DiscountVal == null)
                {
                    cart.DiscountVal = vaucher.Value;
                }
                else cart.DiscountVal += vaucher.Value;
            }

            if (vaucher.Percentage != null)
            {
                if (cart.DiscountPer == null)
                {
                    cart.DiscountPer = vaucher.Percentage;
                }
                else cart.DiscountPer += vaucher.Percentage;
            }
            await CalculaTetoatalAsync();



            if (ViewBag.total < 0)
            {
                TempData["error"] = "The value of discount can`t be greater then subtotal!";
                return RedirectToAction("Index");
            }

            if (cart.DiscountPer > 50)
            {
                TempData["error"] = "The value of percentage discount can`t be greater then 50!";
                return RedirectToAction("Index");
            }


            try
            {
                _context.Update(cart);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartsExists(cart.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            vaucher.IsUsed = true;
            
            try
            {
                _context.Update(vaucher);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VaucherExists(vaucher.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["succes"] = "Vaucher was applied";
            return RedirectToAction("index");

        }


        public async Task<IActionResult> Delete(int id)
        {

            var cartDetails = await _context.CartDetails
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
            await CalculaTetoatalAsync();
            var price = cartDetails.Product.FinalPrice * cartDetails.Quantity ;


            if (ViewBag.Total - price< 0)
            {
                await deleteValDiscountAsync();
            }
            _context.CartDetails.Remove(cartDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }



        public async Task<IActionResult> CheckOut()
        {
            var cart = await GetCurrentUserCartAsync();
            var customer = await GetCustomerByUserIdAsync();
            await CalculaTetoatalAsync();
            var cartDetails = _context.CartDetails
              .Where(c => c.CartId == cart.Id)
              .Include(c => c.Product);
            
            foreach(var item in cartDetails)
            {
                if (item.Quantity>item.Product.Quantity)
                {
                    @TempData["CartMessage"] = "Product " + item.Product.Name+ " stock was exceeded";
                    return RedirectToAction("index");
                }
            }
            if (cartDetails.Count() < 1)
            {
                @TempData["CartMessage"] = "Cart is empty";
                return RedirectToAction("index");

            }
            if(customer.Credit< ViewBag.total)
            {
                @TempData["CartMessage"] = "Credit exceeded";
                return RedirectToAction("index");
            }

            Orders order = new Orders();
            order.OrderStatusId = 8;
            order.CustomerId = customer.Id;
            DateTime date = DateTime.Now;
            order.Date = date;
            order.isPaid = false;
            order.DiscountVal = ViewBag.discountVal + ViewBag.discPackage;
            if (order.DiscountVal == 0)
            {
                order.DiscountVal = null;
            }
            order.TotalPrice = ViewBag.total;
            _context.Add(order);
            await _context.SaveChangesAsync();

            var id = order.Id;
            
            foreach (var item in cartDetails)
            {
                OrderDetails orderDetails = new OrderDetails();
                Products product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                product.Quantity = product.Quantity - item.Quantity;
                orderDetails.OrderId = order.Id;
                orderDetails.ProductId = item.ProductId;
                orderDetails.Quantity = item.Quantity;
                if (ViewBag.discountPer > 0)
                {
                    orderDetails.DiscountPer = ViewBag.discountPer;
                }
                orderDetails.Price = item.Product.FinalPrice - ViewBag.discountPer* item.Product.FinalPrice/100;
                try
                {
                    _context.Update(product);
                }
                catch
                {
                    return NotFound();
                }
                _context.Add(orderDetails);
                _context.Remove(item);

            }

            await _context.SaveChangesAsync();
            cart.DiscountPer = null;
            cart.DiscountVal = null;
            customer.Credit = customer.Credit - ViewBag.total;
            try
            {
                
                _context.Update(cart);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }

            
           
            return RedirectToAction("details","Orders", new {Id=id });
        }


        private bool CartDetailsExists(int id)
        {
            return _context.CartDetails.Any(e => e.Id == id);
        }
        private bool CartsExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);

        }
        private bool VaucherExists(int id)
        {
            return _context.Vauchers.Any(e => e.Id == id);
        }
        private string getUserID()
        {

            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        private async Task<Customers> GetCustomerByUserIdAsync()
        {
            var userId = getUserID();
            Customers customer = await _context.Customers.FirstOrDefaultAsync(m => m.UserId == userId);
            if (customer == null)
                CustomerNotFound();
            return customer;
        }


        public IActionResult CustomerNotFound()
        {
            return NotFound();
        }

        private async Task<Carts> GetCurrentUserCartAsync()
        {
            var customer = await GetCustomerByUserIdAsync();
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.CutomerId == customer.Id);
            return cart;
        }


        private async Task CalculaTetoatalAsync()
        {
            var cart = await GetCurrentUserCartAsync();



            var cartDetails = _context.CartDetails
                .Where(c => c.CartId == cart.Id)
                .Include(c => c.Cart)
                .Include(c => c.Product);
            decimal discountVal = 0;
            int discountPer = 0;
            decimal subTotal = 0;
            decimal discPackage = 0;
            decimal total = 0;
            foreach (var item in cartDetails)
            {
                var price = item.Quantity * item.Product.FinalPrice;

                if (item.Quantity % item.Product.UnitsPerBox == 0)
                {
                    discPackage += price * 5 / 100; //apply the discount of 5% if you buy whole boxes
                }
                subTotal += price;
            }

            if (cart.DiscountVal != null)
            {
                discountVal = (decimal)cart.DiscountVal;
            }

            if (cart.DiscountPer != null)
            {
                discountPer = (int)cart.DiscountPer;
            }

            total = subTotal - discountVal - discPackage;
            total = total - total * discountPer / 100;

            ViewBag.subtotal = Math.Round(subTotal, 2);
            ViewBag.discPackage = Math.Round(discPackage, 2);
            ViewBag.discountPer = discountPer;
            ViewBag.discountVal = Math.Round(discountVal, 2);
            ViewBag.total = Math.Round(total, 2);
        }

        private async Task deleteValDiscountAsync()
        {
            var cart = await GetCurrentUserCartAsync();
            cart.DiscountVal = null;
            try
            {
                _context.Update(cart);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartsExists(cart.Id))
                {
                    NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


    }
}
