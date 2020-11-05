using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobiSys.Models;


namespace MobiSys.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ProductsController : Controller
    {
        private readonly MobiSysContext _context;
        public string error;

        public ProductsController(MobiSysContext context)
        {
            _context = context;

        }

       
        public async Task<IActionResult> Index(string filterName, string filterManufacturer, string filterCategory)
        {

            var ManufacturersDb = _context.Manufacturers.Select(g => g);
            var CategoryDb = _context.Categories.Select(g => g);
            //creating selectlist for manufacturers
            string[] Manufacturers = new string[ManufacturersDb.ToArray().Length + 1];
            int i = 0;
            foreach (var item in ManufacturersDb)
            {
                i++;
                Manufacturers[i] = item.Name;
            }
            Manufacturers[0] = "Choose";
            ViewBag.Manufacturer = new SelectList(Manufacturers);
            //end

            //creating selectlist for category
            string[] Category = new string[CategoryDb.ToArray().Length + 1];
            i = 0;
            foreach (var item in CategoryDb)
            {
                i++;
                Category[i] = item.Name;
            }
            Category[0] = "Choose";
            ViewBag.Category = new SelectList(Category);
            //end

            var products = from s in _context.Products select s;
            ViewBag.filterName = filterName;
            ViewBag.filterManufacturer = filterManufacturer;

            //start filtring products
            if (!String.IsNullOrEmpty(filterName))
            {
                products = products.Where(a =>
                    a.Name.Contains(filterName) ||
                    a.Details.Contains(filterName));
            }

            if (filterCategory != "Choose" && !String.IsNullOrEmpty(filterCategory))
            {
                products = products.Where(a =>
                    a.Cateogry.Name.Equals(filterCategory));
            }
            if (filterManufacturer != "Choose" && !String.IsNullOrEmpty(filterManufacturer))
            {
                products = products.Where(a =>
                    a.Manufacturer.Name.Equals(filterManufacturer));
            }
            //stop filtring products

            if (filterName != null || filterManufacturer != null || filterCategory != null)
            {
                if (products.ToArray().Length == 0)
                {
                    ViewBag.Results = "Nothing Found";
                }
                else
                {
                    ViewBag.Results = "Found " + products.ToArray().Length.ToString() + " results";
                }
            }
            products = products.Where(p => p.Quantity >= p.UnitsPerBox).Include(p => p.Cateogry).Include(p => p.Manufacturer).Include(p => p.Um);

            return View(await products.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Cateogry)
                .Include(p => p.Manufacturer)
                .Include(p => p.Um)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            ViewBag.ProductId = id;

            if (products.Quantity <= products.UnitsPerBox * 10)
            {
                ViewBag.WarningQuantity = "Only " + products.Quantity.ToString() + " units left!";
            }

            var mobiSysContext = _context.Products.Where(p => p.Quantity >= p.UnitsPerBox).Include(p => p.Cateogry).Include(p => p.Manufacturer).Include(p => p.Um);

            return View(await mobiSysContext.ToListAsync());


        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantityInput)
        {
           

            var product = await _context.Products
                .Include(p => p.Cateogry)
                .Include(p => p.Manufacturer)
                .Include(p => p.Um)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var customer = await GetCustomerByUserIdAsync();
            if (customer == null)
            {
                return NotFound();
            }

            
     


            if (quantityInput <= 0) //validate
            {
                TempData["WarningQuantityInput"] = "Qantity must be greater then 0";
                TempData["danger"] = "alert rounded shadow alert-danger";

                return RedirectToAction("Details", new { ID = id });
            }

            if (quantityInput > product.Quantity) // validate
            {
                TempData["WarningQuantityInput"] = "Quantity exceeded";
                TempData["danger"] = "alert rounded shadow alert-danger";
                return RedirectToAction("Details", new { ID = id });
            }




            var cart = _context.Carts.FirstOrDefault(m => m.CutomerId == customer.Id);
            if (cart == null)
            {
                return NotFound();
            }

            var cartDetailsSql = _context.CartDetails  // select * from cartdetails where productid==id and cartid==cart.id 
               .Where(c => c.ProductId == id && c.CartId == cart.Id);


            CartDetails cartDetails = await cartDetailsSql.FirstOrDefaultAsync(c => c.ProductId == product.Id && c.CartId == cart.Id);
            if (cartDetails == null)
            {
                CartDetails cartDetail = new CartDetails();
                cartDetail.ProductId = product.Id;
                cartDetail.Quantity = quantityInput;
                cartDetail.CartId = cart.Id;
                _context.CartDetails.Add(cartDetail);
                await _context.SaveChangesAsync();
                TempData["SuccesQuantityInput"] = "The product was added to cart";
                TempData["success"] = "alert rounded shadow alert-success";
            }

            if (cartDetails != null)
            {
               
                cartDetails.Quantity += quantityInput;
                try
                {
                    _context.Update(cartDetails);
                    await _context.SaveChangesAsync();
                    TempData["SuccesQuantityInput"] = "The product was added to cart";
                    TempData["success"] = "alert rounded shadow alert-success";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartDetailsExists(cartDetails.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }


            return RedirectToAction("Details", new { ID = id });


        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        private string getUserID()
        {

            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public async Task<Customers> GetCustomerByUserIdAsync()
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
        private bool CartDetailsExists(int id)
        {
            return _context.CartDetails.Any(e => e.Id == id);
        }
    }


}
