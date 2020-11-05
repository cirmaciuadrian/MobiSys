using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MobiSys.Models;

namespace MobiSys.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MobiSysContext _context;

        public HomeController(ILogger<HomeController> logger,
                              MobiSysContext context)
        {
            _logger = logger;
            _context = context;
        }
       

       

        public async Task<IActionResult> Index()
        {

            
            return View();
        }
      
        
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private string getUserID()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public async Task<bool> UserIsNewAsync()
        {
            var userId = getUserID();
            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.UserId == userId);
            if (customer == null)
            {
                return true;
            }
            else return false;
        }
    }
}
