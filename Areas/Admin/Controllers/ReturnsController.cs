using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobiSys.Models;

namespace MobiSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager,Salesman")]
    public class ReturnsController : Controller
    {
        private readonly MobiSysContext _context;

        public ReturnsController(MobiSysContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var mobiSysContext = _context.Returns.Include(r => r.Customer).Include(r => r.Employee).Include(r => r.Order).Include(r => r.ReturnStatus);
            return View(await mobiSysContext.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var returns = await _context.ReturnDetails
                .Include(r => r.Return)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.ReturnId == id);
            if (returns == null)
            {
                return NotFound();
            }
            ViewBag.returnId = id;
            var returnn = _context.Returns.FirstOrDefault(o => o.Id == id);
            var order = _context.Orders.FirstOrDefault(o => o.Id == returnn.OrderId);
            ViewBag.orderId = order.Id;
            var customer = _context.Customers.FirstOrDefault(o => o.Id == order.CustomerId);
            ViewBag.customer = customer.Company;
            return View(returns);
        }

        public IActionResult Accept(int? id)
        {
            if (id == null)
            {
                TempData["Msg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("index");
            }
            var returnn = _context.Returns.Include(o => o.Customer).FirstOrDefault(o => o.Id == id);
            var returnDetails = _context.ReturnDetails.Include(o => o.Product).FirstOrDefault(o => o.ReturnId == id);
            var customer = _context.Customers.FirstOrDefault(o => o.Id == returnn.CustomerId);
            var product = _context.Products.FirstOrDefault(o => o.Id == returnDetails.ProductId);
            var orderDetail = _context.OrderDetails.FirstOrDefault(o => o.ProductId == product.Id && o.OrderId == returnn.OrderId);
            if (returnn == null || returnDetails == null || customer == null || product == null || orderDetail == null || returnn.ReturnStatusId !=1 )
            {
                TempData["Msg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("index");
            }
            customer.Credit += returnn.TotalPrice;
            returnn.ReturnStatusId = 2;
            DateTime now = DateTime.Now;
            DateTime expDate = (DateTime)orderDetail.ExpDate;
            if ((expDate - now).Days > 30)
            {
                product.Quantity += returnDetails.Quantity;
                try
                {
                    _context.Update(product);
                    _context.SaveChanges();
                }
                catch
                {
                    TempData["Msg"] = "Something went wrong, try again later!";
                    TempData["Styling"] = "alert rounded shadow alert-danger";
                    return RedirectToAction("index");
                }
            }
            try
            {
                _context.Update(customer);
                _context.Update(returnn);
                _context.SaveChanges();
            }
            catch
            {
                TempData["Msg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("index");
            }

            TempData["Msg"] = "Return " + returnn.Id + " was accepted";
            TempData["Styling"] = "alert rounded shadow alert-success";
            return RedirectToAction("index");
        }
        public IActionResult Cancel(int? id)
        {
            if (id == null)
            {
                TempData["Msg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("index");
            }
            var returnn = _context.Returns.Include(o => o.Customer).FirstOrDefault(o => o.Id == id);
            if (returnn == null || returnn.ReturnStatusId != 1)
            {
                TempData["Msg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("index");
            }
            returnn.ReturnStatusId = 3;
            try
            {
                
                _context.Update(returnn);
                _context.SaveChanges();
            }
            catch
            {
                TempData["Msg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("index");
            }

            TempData["Msg"] = "Return " + returnn.Id + " was canceled";
            TempData["Styling"] = "alert rounded shadow alert-success";
            return RedirectToAction("index");
        }

        private bool ReturnsExists(int id)
        {
            return _context.Returns.Any(e => e.Id == id);
        }

    }



    
}

