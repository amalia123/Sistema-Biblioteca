using MundoNovo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MundoNovo.DAL
{
    public class BibliotecarioDAO
    {
        private static Context ctx = Singleton.Instance.Context;

        public static List<Bibliotecario> ListarBibliotecarios()
        {
            return ctx.Bibliotecarios.ToList();
        }
        public static Bibliotecario BuscarBibliotecarioPorMatricula(long matricula)
        {
            try
            {
                return ctx.Bibliotecarios.FirstOrDefault(x => x.matricula == matricula);
            }
            catch
            {
                return null;
            }
        }
        public static int Login(Bibliotecario bibliotecario)
        {
            
             Bibliotecario biblioAux = ctx.Bibliotecarios.FirstOrDefault(x => x.login == bibliotecario.login && x.senha == bibliotecario.senha);
             if (biblioAux != null)
             {
              
                return biblioAux.id;
             }
             else
             {
                return 0;
               
            }
          
        }

        public static Bibliotecario BuscarBibliotecarioPorId(int? id)
        {
            return ctx.Bibliotecarios.Find(id);
        }

        public static void CadastrarBibliotecario(Bibliotecario bibliotecario)
        {
            ctx.Bibliotecarios.Add(bibliotecario);
            ctx.SaveChanges();
        }

        public static void EditarBibliotecario(Bibliotecario bibliotecario)
        {
            try
            {
                ctx.Set<Bibliotecario>().Attach(bibliotecario);
                ctx.ChangeTracker.Entries<Bibliotecario>().First(e => e.Entity == bibliotecario).State = EntityState.Modified;
                ctx.SaveChanges();
            }
            catch
            {

            }
        }

        public static void RemoverBibliotecario(Bibliotecario bibliotecario)
        {
            ctx.Bibliotecarios.Remove(bibliotecario);
            ctx.SaveChanges();
        }
    }
}