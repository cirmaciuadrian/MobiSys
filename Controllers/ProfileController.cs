using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobiSys.Models;

namespace MobiSys.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly MobiSysContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public ProfileController(MobiSysContext context,
                                 UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            
        }

       
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var customer = await GetCustomerByUserIdAsync();
            if(this.User.IsInRole("Warehouse Team Leader")|| this.User.IsInRole("Salesman"))
            {
                return RedirectToAction("index", "products", new { area = "Admin" });
            }
                                
            if (customer == null)
            {
               
                
                return RedirectToAction("Create");
            }
            else return RedirectToAction("Edit");


        }



        
        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            var customer = await GetCustomerByUserIdAsync();
            if (customer == null)
            {
                
                
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
                ViewData["TypeId"] = new SelectList(_context.Type, "Id", "Name");
                return View();
            }
            else return RedirectToAction("Edit");
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ContactPerson,PhoneNumber,Mail,Company,Cui,RegistrationNumber,Adress,CityId,Bank,Iban,TypeId")] Customers customers)
        {
            var userId = getUserID();
            customers.UserId = userId;
            
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(userId);
                var result = await userManager.AddToRoleAsync(user, "User");
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }
            customers.IsAccepted = false;
            if (ModelState.IsValid)
            {
               
                
                _context.Add(customers);
                await _context.SaveChangesAsync();
                Customers customer = await GetCustomerByUserIdAsync();
                Carts cart = new Carts();
                cart.CutomerId = customer.Id;
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", customers.CityId);
            ViewData["TypeId"] = new SelectList(_context.Type, "Id", "Name", customers.TypeId);
            return View(customers);
        }


       

        public async Task<IActionResult> Edit()
        {

            var userId = getUserID();
            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.UserId == userId);
            
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", customer.CityId);
            ViewData["TypeId"] = new SelectList(_context.Type, "Id", "Name", customer.TypeId);
            return View(customer);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContactPerson,PhoneNumber,Mail,Company,Cui,RegistrationNumber,Adress,CityId,Bank,Iban,TypeId")] Customers customers)
        {
            var userId = getUserID();
            customers.UserId = userId;
            ModelState.Remove("UserId");

            if (id != customers.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", customers.CityId);
            ViewData["TypeId"] = new SelectList(_context.Type, "Id", "Name", customers.TypeId);
            return View(customers);
        }

      

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
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

    }
}
