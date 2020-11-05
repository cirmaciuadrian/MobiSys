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
    public class PromotionsController : Controller
    {
        private readonly MobiSysContext _context;

        public PromotionsController(MobiSysContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Promotions.ToListAsync());
        }

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotions = await _context.Promotions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotions == null)
            {
                return NotFound();
            }

            return View(promotions);
        }

      
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Details")] Promotions promotions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(promotions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(promotions);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotions = await _context.Promotions.FindAsync(id);
            if (promotions == null)
            {
                return NotFound();
            }
            return View(promotions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Details")] Promotions promotions)
        {
            if (id != promotions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionsExists(promotions.Id))
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
            return View(promotions);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotions = await _context.Promotions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotions == null)
            {
                return NotFound();
            }

            return View(promotions);
        }

   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotions = await _context.Promotions.FindAsync(id);
            _context.Promotions.Remove(promotions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionsExists(int id)
        {
            return _context.Promotions.Any(e => e.Id == id);
        }
    }
}
