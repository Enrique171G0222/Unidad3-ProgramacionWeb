using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZooPlanet.Models;
using ZooPlanet.Models.ViewModels;
using ZooPlanet.Repositories;

namespace ZooPlanet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdministradorController : Controller
    {
        public IWebHostEnvironment Environment { get; set; }
        public AdministradorController(IWebHostEnvironment env)
        {
            Environment = env;
        }
        public IActionResult Index()
        {
            animalesContext context = new animalesContext();
            EspeciesRepository er = new EspeciesRepository(context);
            return View(er.GetAll());
        }

        public IActionResult Agregar()
        {
            EspeciesViewModel evm = new EspeciesViewModel();
            animalesContext context = new animalesContext();
            ClasesRepository cr = new ClasesRepository(context);
            evm.Clases = cr.GetAll();
            return View(evm);
        }
        [HttpPost]
        public IActionResult Agregar(EspeciesViewModel evm)
        {
            animalesContext context = new animalesContext();
            try
            {
                ClasesRepository cr = new ClasesRepository(context);
                evm.Clases = cr.GetAll();
                EspeciesRepository er = new EspeciesRepository(context);
                er.Insert(evm.Especie);
                return RedirectToAction("Index", "Administrador", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ClasesRepository cr = new ClasesRepository(context);
                evm.Clases = cr.GetAll();
                return View(evm);
            }
            
        }

        public IActionResult Editar(int id)
        {
            animalesContext context = new animalesContext();
            EspeciesViewModel evm = new EspeciesViewModel();
            EspeciesRepository er = new EspeciesRepository(context);
            evm.Especie = er.GetById(id);
            if (evm.Especie==null)
            {
                return RedirectToAction("Index", "Administrador", new { area = "Admin" });
            }
            ClasesRepository cr = new ClasesRepository(context);
            evm.Clases = cr.GetAll();
            return View(evm);
        }
        [HttpPost]
        public IActionResult Editar(EspeciesViewModel evm)
        {
            animalesContext context = new animalesContext();
            try
            {
                ClasesRepository cr = new ClasesRepository(context);
                evm.Clases = cr.GetAll();
                EspeciesRepository er = new EspeciesRepository(context);
                var e = er.GetById(evm.Especie.Id);
                if (e!=null)
                {
                    e.Especie = evm.Especie.Especie;
                    e.IdClase = evm.Especie.IdClase;
                    e.Habitat = evm.Especie.Habitat;
                    e.Peso = evm.Especie.Peso;
                    e.Tamaño = evm.Especie.Tamaño;
                    e.Observaciones = evm.Especie.Observaciones;
                    er.Update(e);
                }
                return RedirectToAction("Index", "Administrador", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ClasesRepository cr = new ClasesRepository(context);
                evm.Clases = cr.GetAll();
                return View(evm);
            }
        }

        public IActionResult Eliminar(int id)
        {
            using (animalesContext context=new animalesContext())
            {
                EspeciesRepository er = new EspeciesRepository(context);
                var e = er.GetEspecieById(id);
                if (e != null)
                {
                    return View(e);
                }
                else
                    return RedirectToAction("Index", "Administrador", new { area = "Admin" });
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Especies e)
        {
            using (animalesContext context = new animalesContext())
            {
                EspeciesRepository er = new EspeciesRepository(context);
                var especie = er.GetById(e.Id);
                if (especie != null)
                {
                    er.Delete(especie);
                    return RedirectToAction("Index", "Administrador", new { area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError("", "Esta especie no existe o ya ha sido eliminado");
                    return View(e);
                }
            }
        }

        public IActionResult Imagen(int id)
        {
            animalesContext context = new animalesContext();
            EspeciesViewModel evm = new EspeciesViewModel();
            EspeciesRepository er = new EspeciesRepository(context);
            evm.Especie = er.GetById(id);
            if (evm.Especie == null)
            {
                return RedirectToAction("Index", "Administrador", new { area = "Admin" });
            }
            ClasesRepository cr = new ClasesRepository(context);
            evm.Clases = cr.GetAll();
            if (System.IO.File.Exists(Environment.WebRootPath + $"/especies/{evm.Especie.Id}.jpg"))
            {
                evm.Imagen = evm.Especie.Id + ".jpg";
            }
            else
            {
                evm.Imagen = "no-disponible.png";
            }
            return View(evm);
        }
        [HttpPost]
        public IActionResult Imagen(EspeciesViewModel evm)
        {
            animalesContext context = new animalesContext();
            if (evm.Archivo!=null)
            {
                if (evm.Archivo.ContentType != "image/jpeg" || evm.Archivo.Length > 1024 * 1024 * 2)
                {
                    ModelState.AddModelError("", "Debe insertar un archivo jpg de menos de 2Mb");
                    ClasesRepository cr = new ClasesRepository(context);
                    evm.Clases = cr.GetAll();
                    return View(evm);
                }
            }
            try
            {
                EspeciesRepository er = new EspeciesRepository(context);
                var i = er.GetById(evm.Especie.Id);
                if (i != null)
                {
                    if (evm.Archivo!=null)
                    {
                        FileStream fs = new FileStream(Environment.WebRootPath + "/especies/" + evm.Especie.Id + ".jpg", FileMode.Create);
                        evm.Archivo.CopyTo(fs);
                        fs.Close();
                    }
                }
                return RedirectToAction("Index", "Administrador", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ClasesRepository cr = new ClasesRepository(context);
                evm.Clases = cr.GetAll();
                return View(evm);
            }
        }
    }
}