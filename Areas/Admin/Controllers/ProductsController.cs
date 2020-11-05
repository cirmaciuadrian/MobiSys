using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobiSys.Areas.Admin.ViewModels;
using MobiSys.Models;

namespace MobiSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Warehouse Team Leader,Manager,Salesman")]
    public class ProductsController : Controller
    {
        private readonly MobiSysContext _context;
        private readonly IWebHostEnvironment hostingEnviroment;

        public ProductsController(MobiSysContext context,
                                  IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnviroment = hostingEnvironment;
        }

      
        public async Task<IActionResult> Index()
        {
            
            var mobiSysContext = _context.Products.Include(p => p.Cateogry).Include(p => p.Manufacturer).Include(p => p.Um);
           
            return View(await mobiSysContext.ToListAsync());
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

            return View(products);
        }

       
        public IActionResult Create()
        {
            ViewData["CateogryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name");
            ViewData["UmId"] = new SelectList(_context.Um, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ProductsViewModel products)
        {
            if (ModelState.IsValid)
            {
                string uniquFileName = null;
                if (products.photo != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnviroment.WebRootPath, "images");
                    uniquFileName = Guid.NewGuid().ToString() + "_" + products.Name + "_" + products.photo.FileName;
                    string filePath= Path.Combine(uploadsFolder, uniquFileName);       
                    products.photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    
                   
                }
                Products product = new Products
                {
                    ProductCode = products.ProductCode,
                    Name = products.Name,
                    Weight = products.Weight,
                    UmId = products.UmId,
                    InitialPrice = products.InitialPrice,
                    Discount = products.Discount,
                    FinalPrice = products.FinalPrice,
                    Quantity = products.Quantity,
                    Details = products.Details,
                    UnitsPerBox = products.UnitsPerBox,
                    CateogryId = products.CateogryId,
                    ManufacturerId = products.ManufacturerId,
                    Image = uniquFileName
                };
              
                product.FinalPrice = product.InitialPrice - product.Discount;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("details", new { id = product.Id });


            }

            ViewData["CateogryId"] = new SelectList(_context.Categories, "Id", "Name", products.CateogryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", products.ManufacturerId);
            ViewData["UmId"] = new SelectList(_context.Um, "Id", "Name", products.UmId);
            return View(products);
        }

        public async Task<IActionResult> AddQuantity(int? ProductId, int? Quantity)
        {
            if (ProductId == null)
            {

                return NotFound();
            }
            if (Quantity <= 0 || Quantity == null)
            {
                TempData["Msg"] = "Quantity must be greater than 0!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("details", new { id = ProductId });
            }
            var product = _context.Products.FirstOrDefault(o => o.Id == ProductId);
            if (product == null)
            {
                TempData["Msg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("details", new { id = ProductId });
            }
            product.Quantity += (int)Quantity;
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                TempData["Msg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("details", new { id = ProductId });
            }
            TempData["Msg"] = "Quantity updated!";
            TempData["Styling"] = "alert rounded shadow alert-success";
            return RedirectToAction("details", new { id = ProductId });
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            EditProductViewModel product = new EditProductViewModel
            {
                Id = products.Id,
                ProductCode = products.ProductCode,
                Name = products.Name,
                Weight = products.Weight,
                UmId = products.UmId,
                InitialPrice = products.InitialPrice,
                Discount = products.Discount,
                FinalPrice = products.FinalPrice,
                Quantity = products.Quantity,
                Details = products.Details,
                UnitsPerBox = products.UnitsPerBox,
                CateogryId = products.CateogryId,
                ManufacturerId = products.ManufacturerId,
                Image = products.Image
            };


            ViewData["CateogryId"] = new SelectList(_context.Categories, "Id", "Name", products.CateogryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", products.ManufacturerId);
            ViewData["UmId"] = new SelectList(_context.Um, "Id", "Name", products.UmId);
            return View(product);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  EditProductViewModel products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }
            string uniquFileName = null;
            if (ModelState.IsValid)
            {
                if (products.photo != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnviroment.WebRootPath, "images");
                    uniquFileName = Guid.NewGuid().ToString() + "_" + products.Name + "_" + products.photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniquFileName);
                    products.photo.CopyTo(new FileStream(filePath, FileMode.Create));


                }
                if (uniquFileName == null)
                {
                    uniquFileName = products.Image;
                }
                Products product = new Products
                {
                    Id = products.Id,
                    ProductCode = products.ProductCode,
                    Name = products.Name,
                    Weight = products.Weight,
                    UmId = products.UmId,
                    InitialPrice = products.InitialPrice,
                    Discount = products.Discount,
                    FinalPrice = products.FinalPrice,
                    Quantity = products.Quantity,
                    Details = products.Details,
                    UnitsPerBox = products.UnitsPerBox,
                    CateogryId = products.CateogryId,
                    ManufacturerId = products.ManufacturerId,
                    Image = uniquFileName
                };

                //var prodDB = await _context.Products.FirstOrDefaultAsync(o => o.Id == products.Id);
                //if (prodDB.FinalPrice != product.FinalPrice)
                //{
                //    ProductRepricing prodR = new ProductRepricing();
                //    prodR.ProductId = prodDB.Id;
                //    prodR.Price = prodDB.FinalPrice;
                //    DateTime date = DateTime.Now.Date;
                //    prodR.Date = date;
                //    _context.Add(prodR);
                //    await _context.SaveChangesAsync();
                //}
                product.FinalPrice = product.InitialPrice - product.Discount;
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("details", new { id = id });
            }
            ViewData["CateogryId"] = new SelectList(_context.Categories, "Id", "Name", products.CateogryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", products.ManufacturerId);
            ViewData["UmId"] = new SelectList(_context.Um, "Id", "Name", products.UmId);
            return View(products);
        }

        
        
        



        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
