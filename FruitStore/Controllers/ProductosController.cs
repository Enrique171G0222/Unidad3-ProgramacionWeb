using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FruitStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FruitStore.Models;
using FruitStore.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FruitStore.Controllers
{
    public class ProductosController : Controller
    {
        public IHostingEnvironment Environment { get; set; }
        public ProductosController(IHostingEnvironment env)
        {
            Environment = env;
        }
        [Route("Productos")]
        public IActionResult Index()
        {
            ProductosIndexViewModel pivm = new ProductosIndexViewModel();
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository cr = new CategoriasRepository(context);
            ProductosRepository pr = new ProductosRepository(context);
            int? id = null;
            pivm.Categorias = cr.GetAll();
            pivm.Productos = pr.GetProductosByCategoria(id);
            return View(pivm);
        }
        [HttpPost]
        public IActionResult Index(ProductosIndexViewModel pivm)
        {
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository cr = new CategoriasRepository(context);
            ProductosRepository pr = new ProductosRepository(context);
            
            pivm.Categorias = cr.GetAll();
            pivm.Productos = pr.GetProductosByCategoria(pivm.IdCategoria);
            return View(pivm);
        }

        public IActionResult Agregar()
        {
            ProductosViewModel pvm = new ProductosViewModel();
            fruteriashopContext context = new fruteriashopContext();
            CategoriasRepository repos = new CategoriasRepository(context);
            pvm.Categorias = repos.GetAll();
            return View(pvm);
        }
        [HttpPost]
        public IActionResult Agregar(ProductosViewModel pvm)
        {
            fruteriashopContext context = new fruteriashopContext();
            if (pvm.Archivo.ContentType != "image/jpeg" || pvm.Archivo.Length > 1024 * 1024 * 2)
            {
                ModelState.AddModelError("", "Debe insertar un archivo jpg de menos de 2Mb");
                CategoriasRepository repos = new CategoriasRepository(context);
                pvm.Categorias = repos.GetAll();
                return View(pvm);
            }
            try
            {
                ProductosRepository repos = new ProductosRepository(context);
                repos.Insert(pvm.Producto);
                FileStream fs = new FileStream(Environment.WebRootPath + "/img_frutas/" + pvm.Producto.Id + ".jpg", FileMode.Create);
                pvm.Archivo.CopyTo(fs);
                fs.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CategoriasRepository repos = new CategoriasRepository(context);
                pvm.Categorias = repos.GetAll();
                return View(pvm);
            }
            
        }

        public IActionResult Editar(int id)
        {
            fruteriashopContext context = new fruteriashopContext();
            ProductosViewModel pvm = new ProductosViewModel();
            ProductosRepository pr = new ProductosRepository(context);
            pvm.Producto = pr.Get(id);
            if (pvm.Producto==null)
            {
                return RedirectToAction("Index");
            }
            CategoriasRepository cr = new CategoriasRepository(context);
            pvm.Categorias = cr.GetAll();
            if (System.IO.File.Exists(Environment.WebRootPath+$"/img_frutas/{pvm.Producto.Id}.jpg"))
            {
                pvm.Imagen = pvm.Producto.Id + ".jpg";
            }
            else
            {
                pvm.Imagen = "no-disponible.png";
            }
            return View(pvm);
        }
        [HttpPost]
        public IActionResult Editar(ProductosViewModel pvm)
        {
            fruteriashopContext context = new fruteriashopContext();
            if (pvm.Archivo!=null)
            {
                if (pvm.Archivo.ContentType != "image/jpeg" || pvm.Archivo.Length > 1024 * 1024 * 2)
                {
                    ModelState.AddModelError("", "Debe insertar un archivo jpg de menos de 2Mb");
                    CategoriasRepository repos = new CategoriasRepository(context);
                    pvm.Categorias = repos.GetAll();
                    return View(pvm);
                }
            }
            try
            {
                ProductosRepository repos = new ProductosRepository(context);
                var p = repos.Get(pvm.Producto.Id);
                if (p!=null)
                {
                    p.Nombre = pvm.Producto.Nombre;
                    p.IdCategoria = pvm.Producto.IdCategoria;
                    p.UnidadMedida = pvm.Producto.UnidadMedida;
                    p.Precio = pvm.Producto.Precio;
                    p.Descripcion = pvm.Producto.Descripcion;
                    repos.Update(p);

                    if (pvm.Archivo!=null)
                    {
                        FileStream fs = new FileStream(Environment.WebRootPath + "/img_frutas/" + pvm.Producto.Id + ".jpg", FileMode.Create);
                        pvm.Archivo.CopyTo(fs);
                        fs.Close();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CategoriasRepository repos = new CategoriasRepository(context);
                pvm.Categorias = repos.GetAll();
                return View(pvm);
            }
        }

        public IActionResult Eliminar(int id)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                var p = repos.Get(id);
                if (p!=null)
                {
                    return View(p);
                }
                else
                    return Redirect("Index");
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Productos p)
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                var prod = repos.Get(p.Id);
                if (prod!=null)
                {
                    repos.Delete(prod);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "El producto no existe o ya ha sido eliminado");
                    return View(p);
                }
            }
        }
    }

}