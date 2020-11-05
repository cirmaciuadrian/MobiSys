using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobiSys.Areas.Admin.ViewModels;
using MobiSys.Models;

namespace MobiSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Warehouse Team Leader,Manager,Salesman")]
    public class OrdersController : Controller
    {
        private readonly MobiSysContext _context;

        public OrdersController(MobiSysContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var mobiSysContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus);
            return View(await mobiSysContext.ToListAsync());
        }


        public async Task<IActionResult> ViewPending()
        {
            var mobiSysContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Where(o => o.OrderStatus.Status == "Sent");
            return View("index", await mobiSysContext.ToListAsync());
        }
        public async Task<IActionResult> ViewAccepted()
        {
            var mobiSysContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Where(o => o.OrderStatus.Status == "Accepted");
            return View("index", await mobiSysContext.ToListAsync());
        }
        public async Task<IActionResult> ViewCanceled()
        {
            var mobiSysContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Where(o => o.OrderStatus.Status == "Canceled");
            return View("index", await mobiSysContext.ToListAsync());
        }
        public async Task<IActionResult> ViewDelivered()
        {
            var mobiSysContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Where(o => o.OrderStatus.Status == "Delivered");
            return View("index", await mobiSysContext.ToListAsync());
        }
        public async Task<IActionResult> ViewPaid()
        {
            var mobiSysContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Where(o => o.OrderStatus.Status == "Paid");
            return View("index", await mobiSysContext.ToListAsync());
        }
        public async Task<IActionResult> ViewDelivering()
        {
            var mobiSysContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Where(o => o.OrderStatus.Status == "Delivering");
            return View("index", await mobiSysContext.ToListAsync());
        }




        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = _context.OrderDetails.Include(o => o.Order).Include(o => o.Product).Where(o => o.OrderId == id);


            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(await orderDetails.ToListAsync());
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orderDetails = _context.OrderDetails.Include(o => o.Order).Include(o => o.Product).Where(o => o.OrderId == id);
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (orders == null)
            {
                return NotFound();
            }
            if (orders.OrderStatus.Status != "Sent")
            {
                return NotFound();
            }
            ViewBag.Order = id;
            ViewBag.customer = orders.Customer.Company;
            return View(await orderDetails.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, IEnumerable<OrderDetails> details)
        {

            var orders = await _context.Orders
              .Include(o => o.Customer)
              .Include(o => o.Employee)
              .Include(o => o.OrderStatus)
              .FirstOrDefaultAsync(o => o.Id == id);

            if (orders == null || orders.OrderStatusId != 8)
            {
                return NotFound();

            }


            foreach (var item in details)
            {
                var orderDb = await _context.OrderDetails.FirstOrDefaultAsync(o => o.ProductId == item.ProductId && o.OrderId == item.OrderId);
                if (item.Quantity != orderDb.Quantity)
                {
                    var prod = await _context.Products.FirstOrDefaultAsync(o => o.Id == orderDb.ProductId);

                    prod.Quantity = prod.Quantity - (item.Quantity - orderDb.Quantity);
                    if (prod.Quantity < 0)
                    {
                        ViewBag.Order = id;
                        ViewBag.customer = orders.Customer.Company;
                        ViewBag.Error = "Quantity exceeded on product " + prod.Name;
                        var orderview = _context.OrderDetails.Include(o => o.Order).Include(o => o.Product).Where(o => o.OrderId == id);
                        return View(await orderview.ToListAsync());
                    }
                    orderDb.Quantity = item.Quantity;

                    try
                    {
                        _context.Update(orderDb);
                        _context.Update(prod);
                    }
                    catch
                    {
                        return NotFound();
                    }


                }

            }
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateDeliver(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var order = await _context.Orders.Include(o=>o.Customer).FirstOrDefaultAsync(o => o.Id == id) ;
            if (order == null || order.OrderStatusId!=9)
            {
                return NotFound();
            }
            var orderDetails =  _context.OrderDetails.Where(o => o.OrderId == order.Id).Include(o => o.Product);
            IList <OrderDetails> orderDetailsList = await orderDetails.ToListAsync();
            IList<CreateDeliverViewModel> deliver = new List<CreateDeliverViewModel>();
          
            for(int i = 0; i < orderDetailsList.Count; i++)
            {              
                CreateDeliverViewModel temp = new CreateDeliverViewModel();
                temp.productId   = orderDetailsList[i].ProductId;
                temp.productName = orderDetailsList[i].Product.Name;
                temp.OrderId = orderDetailsList[i].OrderId;
                deliver.Add(temp);
            }
            ViewBag.Cars = new SelectList(_context.Cars, "Id", "RegistrationPlate");
            ViewBag.Order = order.Id;
            ViewBag.customer = order.Customer.Company;
            return View(deliver);
        }
       

        [HttpPost]
        public async Task<IActionResult> CreateDeliver(int? id, IEnumerable<CreateDeliverViewModel> deliverRecived)
        {
            if (id == null)
            {
                return NotFound();

            }
            var order = await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null || order.OrderStatusId != 9)
            {
                return NotFound();
            }
            var orderDetails = _context.OrderDetails.Where(o => o.OrderId == order.Id).Include(o => o.Product);
            DateTime currentDate = DateTime.Now.Date;
            bool errorFound = false;
            int carid = 0;
            DateTime? shippingDate = null;
            foreach(var item in deliverRecived)
            {
                if (item.ExpDate < currentDate)
                {
                    errorFound = true;
                    ViewBag.error = "Date must be from future";
                }
                if (item.CarId != null)
                {
                    carid = (int)item.CarId;
                }
                if (item.ShippingDate != null)
                {
                    shippingDate = item.ShippingDate;
                }
                OrderDetails ord = await _context.OrderDetails.FirstOrDefaultAsync(o => o.OrderId == order.Id && o.ProductId == item.productId);
                ord.ExpDate = item.ExpDate;
                try
                {
                    _context.Update(ord);
                    await _context.SaveChangesAsync();

                }
                catch
                {
                    return NotFound();
                }
            }
            if (carid == 0)
            {
                errorFound = true;
                ViewBag.carError = "Select a car";
            }
            if (shippingDate == null || shippingDate<=currentDate)
            {
                errorFound = true;
                ViewBag.shippingEroor = "Shipping date must be from future";
            }

            if (errorFound)
            {
                IList<OrderDetails> orderDetailsList = await orderDetails.ToListAsync();
                IList<CreateDeliverViewModel> deliver = new List<CreateDeliverViewModel>();
                for (int i = 0; i < orderDetailsList.Count; i++)
                {
                    CreateDeliverViewModel temp = new CreateDeliverViewModel();
                    temp.productId = orderDetailsList[i].ProductId;
                    temp.productName = orderDetailsList[i].Product.Name;
                    temp.OrderId = orderDetailsList[i].OrderId;
                    deliver.Add(temp);
                }
                ViewBag.Cars = new SelectList(_context.Cars, "Id", "RegistrationPlate");
                ViewBag.Order = order.Id;
                ViewBag.customer = order.Customer.Company;

                return View(deliver);
            }
            Delivers newDeliver = new Delivers();
            newDeliver.OrderId = order.Id;
            var employee = await GetEmployeeByUserIdAsync();
            newDeliver.EmployeeId = employee.Id;
            newDeliver.CustomerId = (int)order.CustomerId;
            newDeliver.CarId = carid;
            newDeliver.StatusDeliverId = 1;
            _context.Add(newDeliver);
            await _context.SaveChangesAsync();
           
            foreach(var item in orderDetails)
            {
                DeliverDetails temp = new DeliverDetails();
                temp.ProductId = item.ProductId;
                temp.Quantity = item.Quantity;
                temp.DeliverId = newDeliver.Id;
                _context.Add(temp);
            }
            order.ShippingDate = shippingDate;
            order.OrderStatusId = 14;
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }

            return RedirectToAction("index", "Delivers");
       
           
        }

        public async Task<IActionResult> AcceptOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == id);
            if (order.OrderStatus.Status != "Sent")
            {
                return NotFound();
            }
            order.OrderStatusId = 9;
            var employee = await GetEmployeeByUserIdAsync();
            order.EmployeeId = employee.Id;
            try
            {
                TempData["successMsg"] = "Order " + order.Id.ToString() + " was accepted";
                _context.Update(order);
                _context.SaveChanges();

            }
            catch
            {
                return NotFound();
            }

            //TempData["Warning"] = "";
            //TempData["danger"] = "alert rounded shadow alert-danger";


            TempData["success"] = "alert rounded shadow alert-success";
            return RedirectToAction("index");

        }
        public async Task<IActionResult> CancelOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == id);
            if (order.OrderStatus.Status != "Sent" && order.OrderStatus.Status != "Accepted")
            {
                return NotFound();
            }


            var customer = await _context.Customers.FirstOrDefaultAsync(o => o.Id == order.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }
            customer.Credit += order.TotalPrice;
            foreach (var item in order.OrderDetails)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                product.Quantity += item.Quantity;
                try
                {
                    _context.Update(product);
                }
                catch
                {
                    return NotFound();
                }
            }
            var employee = await GetEmployeeByUserIdAsync();
            order.OrderStatusId = 10;
            order.EmployeeId = employee.Id;
            try
            {
                TempData["successMsg"] = "Order " + order.Id.ToString() + " was canceled";
                _context.Update(customer);
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }

            //TempData["Warning"] = "";
            //TempData["danger"] = "alert rounded shadow alert-danger";


            TempData["success"] = "alert rounded shadow alert-success";
            return RedirectToAction("index");

        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
        private string getUserID()
        {

            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public async Task<Employees> GetEmployeeByUserIdAsync()
        {
            var userId = getUserID();
            Employees employee = await _context.Employees.FirstOrDefaultAsync(m => m.UserId == userId);
            return employee;
        }

    }
}
