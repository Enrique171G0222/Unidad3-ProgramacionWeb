using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FruitStore.Controllers
{
    [Route("Admin")]
    [Route("Admin/Index")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}