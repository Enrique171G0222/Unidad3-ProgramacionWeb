using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FruitStore.Models;

namespace FruitStore.Repositories
{
    public class CategoriasRepository:Repository<Categorias>
    {
        public CategoriasRepository(fruteriashopContext context) : base(context)
        {

        }
        public override bool Validate(Categorias entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
            {
                throw new Exception("No escribio un nombre en la categoría");
            }
            if (Context.Categorias.Any(x=>x.Nombre==entidad.Nombre && x.Id!=entidad.Id))
            {
                throw new Exception("La categoría que escribío ya existe");
            }
            return true;
        }
    }
}
