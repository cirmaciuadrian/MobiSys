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
    [Authorize(Roles = "Manager")]
    public class ProductRepricingController : Controller
    {
        private readonly MobiSysContext _context;

        public ProductRepricingController(MobiSysContext context)
        {
            _context = context;
        }

    
        public async Task<IActionResult> Index()
        {
            var mobiSysContext = _context.ProductRepricing.Include(p => p.Product);
            return View(await mobiSysContext.ToListAsync());
        }

      
    }
}
