using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FruitStore.Models;
using FruitStore.Repositories;

namespace FruitStore.Controllers
{
    public class CategoriasController : Controller
    {
        [Route("Categorias")]
        public IActionResult Index()
        {
            fruteriashopContext context = new fruteriashopContext();
            Repositories.Repository<Categorias> repos = new Repositories.Repository<Categorias>(context);
            return View(repos.GetAll().OrderBy(x=>x.Nombre));
        }

        public IActionResult Agregar()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Agregar(Categorias c)
        {
            c.Eliminado = false;
            try
            {
                fruteriashopContext context = new fruteriashopContext();
                CategoriasRepository repos = new CategoriasRepository(context);
                repos.Insert(c);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(c);
            }
        }

        public IActionResult Editar(int id)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                CategoriasRepository repos = new CategoriasRepository(context);
                var categorias = repos.Get(id);
                if (categorias==null)
                {
                    return RedirectToAction("Index");
                }
                return View(categorias);
            }
        }
        [HttpPost]
        public IActionResult Editar(Categorias c)
        {
            try
            {
                using (fruteriashopContext context = new fruteriashopContext())
                {
                    CategoriasRepository repos = new CategoriasRepository(context);
                    var original = repos.Get(c.Id);
                    if (original!=null)
                    {
                        original.Nombre = c.Nombre;
                        repos.Update(original);
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(c);
            }
        }

        public IActionResult Eliminar(int id)
        {
            using (fruteriashopContext context=new fruteriashopContext())
            {
                CategoriasRepository repos = new CategoriasRepository(context);
                var categorias = repos.Get(id);
                if (categorias==null)
                {
                    return RedirectToAction("Index");
                }
                else
                    return View(categorias);
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Categorias c)
        {
            try
            {
                using (fruteriashopContext context = new fruteriashopContext())
                {
                    CategoriasRepository repos = new CategoriasRepository(context);
                    var categorias = repos.Get(c.Id);
                    repos.Delete(categorias);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(c);
            }
        }
    }
}