using SistemaMundoNovo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaMundoNovo.DAL
{
    public class CategoriaDAO
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static List<Categoria> ListarCategorias()
        {
            return db.Categorias.ToList();
        }

        public static Categoria BuscarCategoriaPorId(int? id)
        {
           
            return db.Categorias.Find(id);
        }
    }
}