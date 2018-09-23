using MundoNovo.DAL;
using MundoNovo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MundoNovo.Controllers
{
    public class LivrosController : Controller
    {
        //Buscar livro
        public ActionResult Busca(string busca)
        {
            List<Livro> livros = LivroDAO.BuscarLivroPorNome(busca);
            if (livros.Count() == 0)
            {
                ViewBag.resultado = "Livro não encontrado";
                ViewBag.TodosLivros = LivroDAO.ListarLivros();
                return View("Index");
            }
            ViewBag.Livros = livros;
            return View(livros);

        }


        // GET: Livros
        public ActionResult Index()
        {
            return View(LivroDAO.ListarLivros());
        }

        // GET: Livros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = LivroDAO.BuscarLivroPorId(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            return View(livro);
        }

        // GET: Livros/Create
        public ActionResult Create()
        {
            ViewBag.Categorias = new SelectList(
                CategoriaDAO.ListarCategorias(),
                "id", "nome");
            return View();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,titulo,autor,ano,descricao")] Livro livro, int? Categorias)
        {
            if (Categorias > 0)
            {
                Categoria categ = CategoriaDAO.BuscarCategoriaPorId(Categorias);
                livro.categoria = categ;
            }

            if (ModelState.IsValid)
            {
                LivroDAO.CadastrarLivro(livro);
                return RedirectToAction("Index");
            }


            return View(livro);
        }

        // GET: Livros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = LivroDAO.BuscarLivroPorId(id);
            if (livro == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categorias = new SelectList(
                CategoriaDAO.ListarCategorias(),
                "id", "nome");
            return View(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,titulo,autor,ano,descricao")] Livro livro, int Categorias)
        {
            Livro livroAux = LivroDAO.BuscarLivroPorId(livro.id);
            livroAux.ano = livro.ano;
            livroAux.autor = livro.autor;
            livroAux.categoria = CategoriaDAO.BuscarCategoriaPorId(Categorias);
            livroAux.descricao = livro.descricao;
            livroAux.titulo = livro.titulo;
            if (ModelState.IsValid)
            {
                //Erro sem tratamento
                LivroDAO.EditarLivro(livroAux);
                return RedirectToAction("Index");
            }
            return View(livroAux);
        }

        // GET: Livros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = LivroDAO.BuscarLivroPorId(id);
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
            Livro livro = LivroDAO.BuscarLivroPorId(id);
            LivroDAO.RemoverLivro(livro);
            return RedirectToAction("Index");
        }
    }
}