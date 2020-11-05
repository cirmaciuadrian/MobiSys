using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobiSys.Models;

namespace MobiSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Warehouse Team Leader,Manager,Salesman")]
    public class DashboardController : Controller
    {
        
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("index","products");
        }
    }
}
