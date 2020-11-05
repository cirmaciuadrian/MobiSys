using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobiSys.Areas.Admin.ViewModels;
using MobiSys.Models;
using Rotativa.AspNetCore;

namespace MobiSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Warehouse Team Leader,Manager")]
    public class DeliversController : Controller
    {
        private readonly MobiSysContext _context;

        public DeliversController(MobiSysContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var mobiSysContext = _context.Delivers.Include(d => d.Car).Include(d => d.Customer).Include(d => d.Employee).Include(d => d.Order).Include(d => d.StatusDeliver);
            return View(await mobiSysContext.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var deliver = await _context.Delivers.Include(o => o.Order).Include(o => o.Car).Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
            if ( deliver == null)
            {
                return NotFound();
            }

            ViewBag.deliver = deliver.Id;
            ViewBag.Customer = deliver.Order.Customer.Company;
            ViewBag.Adress = deliver.Order.Customer.Adress;
            ViewBag.order = deliver.OrderId;
            ViewBag.car = deliver.Car.RegistrationPlate;
            ViewBag.Phone = deliver.Order.Customer.PhoneNumber;

          

            string[] stringArray = new string[6] { deliver.Id.ToString(), deliver.Order.Customer.Company, deliver.Order.Customer.Adress, deliver.OrderId.ToString(), deliver.Car.RegistrationPlate, deliver.Order.Customer.PhoneNumber };
            ViewBag.Content = stringArray;
          

            var mobiSysContext = _context.DeliverDetails.Where(o => o.DeliverId == id).Include(d => d.Deliver).Include(d => d.Product);
            return View(await mobiSysContext.ToListAsync());

        }
         
        public async Task<IActionResult> finishDeliver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var deliver = await _context.Delivers.Include(o => o.StatusDeliver).FirstOrDefaultAsync(o => o.Id == id);
            if (deliver == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.Include(o=>o.OrderStatus).FirstOrDefaultAsync(o => o.Id == deliver.OrderId);
            if (order == null || order.OrderStatus.Status!= "Delivering" || deliver.StatusDeliver.Status != "Ready")
                
            {
                return NotFound();
            }
            order.OrderStatusId = 11;
            deliver.StatusDeliverId = 2;
            try
            {
                TempData["successMsg"] = "Deliver number " + deliver.Id.ToString() + " was finished";
                _context.Update(order);
                _context.Update(deliver);
              
                await _context.SaveChangesAsync();

            }
            catch
            {
                return NotFound();
            }
           
            TempData["success"] = "alert rounded shadow alert-success";
            return RedirectToAction("index");
          

        }


        public async Task<IActionResult> ExportPDF(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var deliver = await _context.Delivers.Include(o => o.Order).Include(o => o.Car).Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
            if ( deliver == null)
            {
                return NotFound();
            }

            string[] stringArray = new string[6] { deliver.Id.ToString(), deliver.Order.Customer.Company, deliver.Order.Customer.Adress, deliver.OrderId.ToString(), deliver.Car.RegistrationPlate, deliver.Order.Customer.PhoneNumber };
            ViewBag.Content = stringArray;
            ViewData["content"] = stringArray;

            var mobiSysContext = _context.DeliverDetails.Where(o => o.DeliverId == id).Include(d => d.Deliver).Include(d => d.Product);
            ViewBag.deliver = deliver.Id;
            ViewBag.Customer = deliver.Order.Customer.Company;
            ViewBag.Adress = deliver.Order.Customer.Adress;
            ViewBag.order = deliver.OrderId;
            ViewBag.car = deliver.Car.RegistrationPlate;
            ViewBag.Phone = deliver.Order.Customer.PhoneNumber;
            IList<PdfViewModel> pdf = new List<PdfViewModel>();
            foreach(var item in mobiSysContext)
            {
                PdfViewModel temp = new PdfViewModel();
                temp.Adress = deliver.Order.Customer.Adress;
                temp.Customer = deliver.Order.Customer.Company;
                temp.DeliverId = deliver.Id;
                temp.OrderId = deliver.OrderId;
                temp.Car = deliver.Car.RegistrationPlate;
                temp.Phone = deliver.Order.Customer.PhoneNumber;
                temp.ProductName = item.Product.Name;
                temp.Weight = item.Product.Weight;
                temp.Quantity = item.Quantity;
                temp.UnitsPerBox = item.Product.UnitsPerBox;
                pdf.Add(temp);
            }
            return new ViewAsPdf("Pdf",pdf);

        }



        private bool DeliversExists(int id)
        {
            return _context.Delivers.Any(e => e.Id == id);
        }
    }
}
