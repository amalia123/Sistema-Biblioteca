using MundoNovo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MundoNovo.DAL
{
    public class LivroDAO
    {
        private static Context ctx = Singleton.Instance.Context;

        public static List<Livro> BuscarLivroPorNome(string busca)
        {
            return ctx.Livros.Where(x => x.titulo.Contains(busca)).ToList();
        }

        public static List<Livro> ListarLivros()
        {
            return ctx.Livros.ToList();
        }

        public static Livro BuscarLivroPorId(int? id)
        {
            return ctx.Livros.Find(id);
        }

        public static void CadastrarLivro(Livro livro)
        {
            ctx.Livros.Add(livro);
            ctx.SaveChanges();
        }

        public static void EditarLivro(Livro livro)
        {
            ctx.Entry(livro).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public static void RemoverLivro(Livro livro)
        {
            ctx.Livros.Remove(livro);
            ctx.SaveChanges();
        }
    }
}