﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SistemaMundoNovo.DAL;
using SistemaMundoNovo.Models;
using SistemaMundoNovo.Utils;

namespace SistemaMundoNovo.Controllers
{
    public class LivrosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private ApplicationDbContext db = Singleton.Instance.Context;
        //Buscar livro
        public ActionResult Busca(string busca)
        {
            ApplicationUser b = UsuarioUtils.RetornaUsuarioLogado();
            int idBibliotecarioLogado = b._Bibliotecario.BibliotecarioID;
            var livros = db.Livros.Where(x => x.titulo.Contains(busca) && x.BibliotecarioID == idBibliotecarioLogado).ToList();
            if (livros.Count() == 0)
            {
                ViewBag.resultado = "Livro não encontrado";
                ViewBag.TodosLivros = db.Livros.Where(x => x.BibliotecarioID == idBibliotecarioLogado).Include(x => x._categoria).ToList();
                return View("Index");
            }
            ViewBag.Livros = livros;
            return View(livros);

        }

        // GET: Livros
        public ActionResult Index()
        {

            ApplicationUser b = UsuarioUtils.RetornaUsuarioLogado();
            int idBibliotecarioLogado = b._Bibliotecario.BibliotecarioID;
            var livroes = db.Livros.Where(x => x.BibliotecarioID == idBibliotecarioLogado).Include(x => x._categoria);
            return View(livroes.ToList());
        }

        // GET: Livros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            return View(livro);
        }

        // GET: Livros/Create
        public ActionResult Create()
        {
           // ViewBag.Categorias = new SelectList( CategoriaDAO.ListarCategorias(),"id", "nome");
            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaId", "Nome");
            //ViewBag.BibliotecarioID = new SelectList(db.Bibliotecarios, "BibliotecarioID", "Nome");
            return View();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,titulo,autor,ano,descricao,BibliotecarioID")] Livro livro, int ? Categorias)
        {

            if (Categorias > 0)
            {
                Categoria categ = CategoriaDAO.BuscarCategoriaPorId(Categorias);
                livro.categoriaId = categ.CategoriaId;
            }

            if (ModelState.IsValid)
            {
                // identificando bibliotecario logado para salvar o livro
                ApplicationUser usuario = UsuarioUtils.RetornaUsuarioLogado();
                int idBibliotecarioLogado = usuario._Bibliotecario.BibliotecarioID;

                livro.BibliotecarioID = idBibliotecarioLogado;

                db.Livros.Add(livro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaId", "Nome", livro._categoria.CategoriaId);
            // ViewBag.BibliotecarioID = new SelectList(db.Bibliotecarios, "BibliotecarioID", "Nome", livro.BibliotecarioID);
            return View(livro);
        }

        // GET: Livros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id.GetValueOrDefault());
            if (livro == null)
            {
                return HttpNotFound();
            }

            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaId", "Nome", livro._categoria.CategoriaId);
            //ViewBag.BibliotecarioID = new SelectList(db.Bibliotecarios, "BibliotecarioID", "Nome", livro.BibliotecarioID);
            return View(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,titulo,autor,ano,descricao,BibliotecarioID")] Livro livro, int Categorias)
        {

            /*ApplicationUser usuario = UsuarioUtils.RetornaUsuarioLogado();
            int idBibliotecarioLogado = usuario._Bibliotecario.BibliotecarioID;*/

            Livro livroAux = db.Livros.Find(livro.id);
            livroAux.ano = livro.ano;
            livroAux.autor = livro.autor;
            livroAux.categoriaId = livro._categoria.CategoriaId;
            livroAux.descricao = livro.descricao;
            //livroAux.BibliotecarioID = livro.BibliotecarioID;
            //livroAux.BibliotecarioID = idBibliotecarioLogado;
            livroAux.titulo = livro.titulo;

            if (ModelState.IsValid)
            {
                // identificando novamente o bibliotecario logado para editar o livro
                ApplicationUser usuario = UsuarioUtils.RetornaUsuarioLogado();
                int idBibliotecarioLogado = usuario._Bibliotecario.BibliotecarioID;
                livro.BibliotecarioID = idBibliotecarioLogado;

                //incluindo a categoria novamente
                Categoria categ = CategoriaDAO.BuscarCategoriaPorId(Categorias);
                livro.categoriaId = categ.CategoriaId;

                db.Entry(livroAux).CurrentValues.SetValues(livro);
                db.Entry(livroAux).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = new SelectList(db.Categorias, "CategoriaId", "Nome", livro._categoria.CategoriaId);
            //ViewBag.BibliotecarioID = new SelectList(db.Bibliotecarios, "BibliotecarioID", "Nome", livro.BibliotecarioID);
            return View(livro);
        }

        // GET: Livros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = db.Livros.Find(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Livro livro = db.Livros.Find(id);
            db.Livros.Remove(livro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
