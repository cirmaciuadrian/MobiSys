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
    [Authorize(Roles = "Manager,Salesman")]
    public class VauchersController : Controller
    {
        private readonly MobiSysContext _context;

        public VauchersController(MobiSysContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Company");

            var mobiSysContext = _context.Vauchers.Include(v => v.CreatedByNavigation).Include(v => v.Customer).Include(v => v.Order);
            return View(await mobiSysContext.ToListAsync());
        }

      
       


        
        [HttpPost]
        public async Task<IActionResult> CreatePercentage(PercentageModelView vaucher)
        {
            
            if (ModelState.IsValid)
            {
               
                string code = await CreatePerCodeAsync(vaucher.Percentage);
                Vauchers perVaucher = new Vauchers();
                perVaucher.Code = code;
                perVaucher.Percentage = vaucher.Percentage;
                Employees emp = await GetEmployeeByUserIdAsync();
                perVaucher.CreatedBy = emp.Id;
                perVaucher.CustomerId = vaucher.CustomerId;
                perVaucher.IsUsed = false;
                _context.Add(perVaucher); 
                 await _context.SaveChangesAsync();
                
                TempData["Styling"] = "alert rounded shadow alert-success";
                TempData["Msg"] = "Voucher " + perVaucher.Code + " added succesfully!";          
                return RedirectToAction(nameof(Index));
               

            }
            TempData["Styling"] = "alert rounded shadow alert-danger";
            TempData["Msg"] = "Percentage must be > 0 and < 50!";
            
            return RedirectToAction("index");
        }



        [HttpPost]
        public async Task<IActionResult> CreateValue(ValueModelView vaucher)
        {

            if (ModelState.IsValid)
            {

                string code = await CreateValCodeAsync(vaucher.Value);
                Vauchers valVaucher = new Vauchers();
                valVaucher.Code = code;
                valVaucher.Value = vaucher.Value;
                Employees emp = await GetEmployeeByUserIdAsync();
                valVaucher.CreatedBy = emp.Id;
                valVaucher.CustomerId = vaucher.CustomerId;
                valVaucher.IsUsed = false;
                _context.Add(valVaucher);
                await _context.SaveChangesAsync();
                
                TempData["Styling"] = "alert rounded shadow alert-success";
                TempData["Msg"] = "Voucher " + valVaucher.Code + " added succesfully!";
                return RedirectToAction(nameof(Index));


            }
            TempData["Styling"] = "alert rounded shadow alert-danger";
            TempData["Msg"] = "Value must be greater then 0";
            
            return RedirectToAction("index");
        }




        
      
        [ActionName("DisableVoucher")]
        public async Task<IActionResult> DisableVoucher(int id)
        {
           var vaucher = await _context.Vauchers.FirstOrDefaultAsync(c => c.Id == id);
           if (vaucher==null)
            {
                return NotFound();
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
            return RedirectToAction(nameof(Index));
        }


        private bool VauchersExists(int id)
        {
            return _context.Vauchers.Any(e => e.Id == id);
        }

        public async Task<string> CreatePerCodeAsync(int percent)
        {
            var currentVauchers = await _context.Vauchers.CountAsync();
            string code = currentVauchers.ToString() + "PER" + percent.ToString();
            return code;
            
        }

        public async Task<string> CreateValCodeAsync(int value)
        {
            var currentVauchers = await _context.Vauchers.CountAsync();
            string code = currentVauchers.ToString() + "VAL" + value.ToString();
            return code;

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

        private bool VaucherExists(int id)
        {
            return _context.Vauchers.Any(e => e.Id == id);
        }
    }
}
