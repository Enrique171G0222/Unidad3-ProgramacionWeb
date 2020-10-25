using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FruitStore.Models;
using FruitStore.Models.ViewModels;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FruitStore.Controllers
{
    public class HomeController : Controller
    {
        [Route("Home/Index")]
        [Route("Home")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("{id}")]
        public IActionResult Categoria(string id)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                CategoriaViewModel cvm = new CategoriaViewModel();
                cvm.Nombre = id;
                cvm.Productos = repos.GetProductosByCategoria(id).ToList();
                return View(cvm);
            }
        }
        [Route("detalles/{categoria}/{id}")]
        public IActionResult Ver(string categoria, string id)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                Productos p = repos.GetProductosByCategoria(categoria, id.Replace("-", " "));
                return View(p);
            }
        }
    }
}