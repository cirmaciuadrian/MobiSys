using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MobiSys.Models;

namespace MobiSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager,Salesman")]
    public class CustomersController : Controller
    {
        private readonly MobiSysContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public CustomersController(MobiSysContext context,
                                    UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }



      



        public async Task<IActionResult> Index(string searchString, string Accepted)
        {
            var customers = from m in _context.Customers
                            select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.Company.Contains(searchString)
                ||s.ContactPerson.Contains(searchString)
                ||s.Cui.Contains(searchString)
                ||s.RegistrationNumber.Contains(searchString));
            }

            if (Accepted == "yes")
            {
                customers = customers.Where(s => s.IsAccepted == true);
            }
            if (Accepted == "no")
            {
                customers = customers.Where(s => s.IsAccepted == false);
            }

            if (customers.ToArray().Length == 0)
            {
                ViewBag.Result = "Nothing found";
            }



            return View(await customers.ToListAsync());

        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .Include(c => c.City)
                .Include(c => c.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", customers.CityId);
            ViewData["TypeId"] = new SelectList(_context.Type, "Id", "Name", customers.TypeId);
            return View(customers);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ContactPerson,PhoneNumber,Mail,Company,Cui,RegistrationNumber,Adress,CityId,Bank,Iban,IsAccepted,Credit,TypeId")] Customers customers)
        {
            if (id != customers.Id)
            {
                return NotFound();
            }

            var UserId = customers.UserId;
            var user = await userManager.FindByIdAsync(UserId);
            string role;
            if (customers.IsAccepted)
            {
                role = "Customer";
            }
            else role = "User";


            if (ModelState.IsValid)
            {

                try
                {
                    var roles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    var result = await userManager.AddToRoleAsync(user, role);
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
    }
}
