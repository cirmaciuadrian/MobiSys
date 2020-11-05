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
    [Authorize(Roles = "Manager")]
    public class EmployeesController : Controller
    {
        private readonly MobiSysContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public EmployeesController(MobiSysContext context,
                                   RoleManager<IdentityRole> roleManager,
                                   UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

      
        public async Task<IActionResult> Index()
        {
            var mobiSysContext = _context.Employees.Include(e => e.City);
            return View(await mobiSysContext.ToListAsync());
        }

       
       

       [HttpGet]
        public IActionResult Create()
        {
            ViewData["JobId"] = new SelectList(roleManager.Roles);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,email,Adress,CityId,JobId,Salary")] Employees employees)
        {    
            var user = await userManager.FindByNameAsync(employees.email);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            ViewData["JobId"] = new SelectList(roleManager.Roles);
            if (user == null)
            {

                ViewBag.Class = "alert alert-danger";
                ViewBag.ErrorMsg = " This email dose not corespond to an user account.";
                return View();

            }
            else
            {
                employees.UserId = user.Id;
                if (_context.Employees.FirstOrDefault(e => e.email == employees.email) != null)
                {
                    ViewBag.Class = "alert alert-danger";
                    ViewBag.ErrorMsg = " This email is already linked to an employee acount.";
                    return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            var result = await userManager.AddToRoleAsync(user, employees.JobId);
                        }
                        catch (Exception)
                        {

                            ViewBag.Class = "alert alert-danger";
                            ViewBag.ErrorMsg = "Role not valid";
                            View(employees);
                        }
                        _context.Add(employees);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    
                    
                    return View(employees);
                }
            }
           
           


        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", employees.CityId);
            ViewData["JobId"] = new SelectList(roleManager.Roles);
            return View(employees);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,FullName,email,Adress,CityId,JobId,Salary")] Employees employees)
        {
            if (id != employees.Id)
            {
                return NotFound();
            }
            var UserId = employees.UserId;
            var user = await userManager.FindByIdAsync(UserId);

            if (ModelState.IsValid)
            {
                try
                {

                    var roles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    var result = await userManager.AddToRoleAsync(user, employees.JobId);
                }
                catch (Exception)
                {

                    ViewBag.Class = "alert alert-danger";
                    ViewBag.ErrorMsg = "Role not valid";
                    return View(employees);
                }
                try
                {
                    _context.Update(employees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employees.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", employees.CityId);
            return View(employees);
        }


        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
