using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobiSys.Models;
using MobiSys.ViewModels;

namespace MobiSys.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrdersController : Controller
    {
        private readonly MobiSysContext _context;

        public OrdersController(MobiSysContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var customer = await GetCustomerByUserIdAsync();
            ViewBag.credit = customer.Credit;
            var orders = _context.Orders.Where(o => o.CustomerId==customer.Id).Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Include(o=> o.OrderDetails);
            //var details = from s in _context.OrderDetails select s;
            
            DateTime current = DateTime.Now.Date;
          
            ViewBag.productName = new List<string>();
            ViewBag.dateExp = new List<string>();
            ViewBag.orderId = new List<int>();
            ViewBag.eproductName = new List<string>();
            ViewBag.edateExp = new List<string>();
            ViewBag.eorderId = new List<int>();
            foreach (var order in orders)
            {
                var details =  _context.OrderDetails.Where(o => o.OrderId==order.Id).Include(o=>o.Product);
                foreach (var detail in details)
                {
                    if (detail.ExpDate != null)
                    {
                        DateTime expDatee = (DateTime)detail.ExpDate;
                        var expDate = expDatee.Date.ToString("dd/MM/yyyy"); 
                        if ((expDatee - current).Days < 10 && (expDatee - current).Days > 0 )
                        {
                          
                            ViewBag.productName.Add(detail.Product.Name);
                            ViewBag.dateExp.Add(expDate);
                            ViewBag.orderId.Add(detail.OrderId);
                        }
                        if ((expDatee - current).Days < 0 )
                        {

                            ViewBag.eproductName.Add(detail.Product.Name);
                            ViewBag.edateExp.Add(expDate);
                            ViewBag.eorderId.Add(detail.OrderId);
                        }
                    }
                }
                if (ViewBag.productName == null)
                {
                    ViewBag.productName = '0';
                    ViewBag.dateExp = '0';
                    ViewBag.orderId = 0;
                    ViewBag.Styl = "hidden";
                }
                if (ViewBag.eproductName == null)
                {
                    ViewBag.eproductName = '0';
                    ViewBag.edateExp = '0';
                    ViewBag.eorderId = 0;
                    ViewBag.Styl = "hidden";
                }
            }

            return View(await orders.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await GetCustomerByUserIdAsync();
            ViewBag.credit = customer.Credit;
            var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
            if (order.CustomerId != customer.Id)
            {
                return NotFound();
            }
            var mobiSysContext = _context.OrderDetails.Where(o=>o.OrderId==id).Include(o => o.Order).Include(o => o.Product);
            ViewBag.Orderid = id;
            IList<ProductViewModel> productList = new List<ProductViewModel>();
            foreach (var item in mobiSysContext)
            {
                ProductViewModel product = new ProductViewModel();
                product.Id = item.ProductId;
                product.Name = item.Product.Name;
                productList.Add(product);
            }
            ViewBag.Products = new SelectList(productList, "Id", "Name");
            
            return View(await mobiSysContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> payment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await GetCustomerByUserIdAsync();
            Orders order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null || order.isPaid || order.OrderStatusId!=11)
            {
                return NotFound();
            }
            customer.Credit = customer.Credit + order.TotalPrice;
            order.OrderStatusId = 13;
            order.isPaid = true;
            try
            {
                _context.Update(customer);
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }
            //return RedirectToAction("index");

            var customers = await GetCustomerByUserIdAsync();
            ViewBag.credit = customers.Credit;
            var mobiSysContext = _context.Orders.Where(o => o.CustomerId == customers.Id).Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus);

            TempData["Msg"] = "Paid succeded!";
            TempData["Styling"] = "alert rounded shadow alert-success";
            return RedirectToAction("index");
        }

        public IActionResult Return(int? OrderId, int? ProductId, int? Quantity, string? Reason)
        {
            if (OrderId == null || ProductId == null)
                return NotFound();
            Orders order = _context.Orders.FirstOrDefault(m => m.Id == OrderId);
            OrderDetails details = _context.OrderDetails.Where(m => m.OrderId == OrderId).Include(o => o.Product).FirstOrDefault(o => o.ProductId == ProductId);
            if (Quantity == 0 || Quantity==null || Reason == null || Quantity>details.Quantity)
            {
                TempData["DangerMsg"] = "Quantity must be greater than 0 but lower then total ordered quantity and reason is required";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("Details", new { id = OrderId });
            }
          
            Returns returnn = new Returns
            {
                CustomerId = (int)order.CustomerId,
                OrderId = order.Id,
                ReturnStatusId = 1,
                TotalPrice = 0
            };
            _context.Add(returnn);
            _context.SaveChanges();
            ReturnDetails returnDetails = new ReturnDetails
            {
                ReturnId = returnn.Id,
                ProductId = (int)ProductId,
                Quantity = (int)Quantity,
                Reason = (string)Reason
            };

            returnn.TotalPrice =  details.Price*returnDetails.Quantity;
            try
            {
                _context.Update(returnn);
                _context.Add(returnDetails);
                _context.SaveChanges();
                TempData["SuccessMsg"] = "Return request was sent";
                TempData["Styling"] = "alert rounded shadow alert-success";
            } 
            catch{

                TempData["DangerMsg"] = "Something went wrong, try again later!";
                TempData["Styling"] = "alert rounded shadow alert-danger";
                return RedirectToAction("Details", new { id = OrderId });

            }


           
            return RedirectToAction("Details", new { id = OrderId });
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
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
    }
}
