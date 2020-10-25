using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FruitStore.Models;

namespace FruitStore.Services
{
    public class CategoriaService
    {
        public List<Categorias> Categorias { get; set; }
        public CategoriaService()
        {
            using (fruteriashopContext context = new fruteriashopContext())
            {
                Repositories.Repository<Categorias> repos = new Repositories.Repository<Categorias>(context);
                Categorias = repos.GetAll().OrderBy(x=>x.Nombre).ToList();
            }
        }
    }
}
