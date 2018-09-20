
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SistemaMundoNovo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaMundoNovo.Utils
{
    public class UsuarioUtils
    {
        

       public static ApplicationUser RetornaUsuarioLogado()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            string chaveUsuarioAspNet = HttpContext.Current.User.Identity.GetUserId();

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var usuarioLogado = manager.FindById(HttpContext.Current.User.Identity.GetUserId());

            return usuarioLogado;
        }

        public static int RetornaIdBibliotecarioLogado()
        {
            ApplicationUser b = UsuarioUtils.RetornaUsuarioLogado();
            return b._Bibliotecario.BibliotecarioID;
        }
    }
}