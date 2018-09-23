using MundoNovo.DAL;
using MundoNovo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MundoNovo.Controllers
{
    public class EmprestimosController : Controller
    {
        // GET: Emprestimos
        public ActionResult Index()
        {
            return View(EmprestimoDAO.RetonarEmprestimos());
        }

        // GET: Emprestimos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emprestimo emprestimo = EmprestimoDAO.BuscarEmprestimoPorId(id);
            if (emprestimo == null)
            {
                return HttpNotFound();
            }
            return View("Darbaixa", emprestimo);
        }

        // POST: Emprestimos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,valor")] Emprestimo emprestimo, int status, DateTime dataDevolucao)
        {
            double novoValor = emprestimo.valor;
            emprestimo = EmprestimoDAO.BuscarEmprestimoPorId(emprestimo.id);
            emprestimo.valor = novoValor;
            emprestimo.status = status;
            emprestimo.dataDevolucao = Convert.ToString(dataDevolucao);
            if (ModelState.IsValid)
            {
                EmprestimoDAO.EditarEmprestimo(emprestimo);
                return RedirectToAction("Index");
            }
            return View(emprestimo);
        }

        [HttpPost]
        public ActionResult PesquisarCep(Emprestimo emprestimo)
        {
            string url = "https://viacep.com.br/ws/" + emprestimo.cep + "/json/";
            WebClient client = new WebClient();
            try
            {
                Emprestimo aux = emprestimo;
                //Consumindo os dados do Viacep
                string resultado = client.DownloadString(@url);
                //Converter para UTF8
                byte[] bytes = Encoding.Default.GetBytes(resultado);
                resultado = Encoding.UTF8.GetString(bytes);
                //Converter os dados da string em objeto
                emprestimo = JsonConvert.DeserializeObject<Emprestimo>(resultado);

                emprestimo.endereco = emprestimo.Logradouro + " " + emprestimo.Localidade;
                emprestimo.livro = aux.livro;
                emprestimo.nome = aux.nome;
                emprestimo.status = aux.status;
                emprestimo.bibliotecario = aux.bibliotecario;
                emprestimo.dataDevolucao = aux.dataDevolucao;
                emprestimo.dataPrazo = aux.dataPrazo;
                emprestimo.id = aux.id;
                emprestimo.cep = aux.cep;
                emprestimo.valor = aux.valor;

                //Passar o objeto preenchido para outra Action
                HttpContext.Session["Emprestimo"] = emprestimo;
            }
            catch (Exception)
            {
                TempData["Mensagem"] = "CEP inválido!";
            }
            return RedirectToAction("Create", "Emprestimos");
        }

        // GET: Emprestimos/Create
        public ActionResult Create(int? id)
        {
            Emprestimo emprestimo = new Emprestimo();
            emprestimo.livro = LivroDAO.BuscarLivroPorId(id);
            return View("Emprestimo", emprestimo);
        }

        // POST: Emprestimos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,livro,valor,cep,endereco")] Emprestimo emprestimo, long matricula, string nome, DateTime dataPrazo)
        {

            string url = "https://viacep.com.br/ws/" + emprestimo.cep + "/json/";
            WebClient client = new WebClient();
            try
            {

                Emprestimo aux = new Emprestimo();
                string resultado = client.DownloadString(@url);
                //Converter para UTF8
                byte[] bytes = Encoding.Default.GetBytes(resultado);
                resultado = Encoding.UTF8.GetString(bytes);
                //Converter os dados da string em objeto
                aux = JsonConvert.DeserializeObject<Emprestimo>(resultado);
                emprestimo.endereco = aux.Logradouro + emprestimo.endereco;
                emprestimo.Bairro = aux.Bairro;
                emprestimo.Localidade = aux.Localidade;
                emprestimo.Uf = aux.Uf;
                emprestimo.Logradouro = aux.Logradouro;
            }
            catch
            {
                emprestimo.cep = "Cep Inválido";
            }

            emprestimo.status = 0;
            emprestimo.nome = nome;
            emprestimo.bibliotecario = BibliotecarioDAO.BuscarBibliotecarioPorMatricula(matricula);
            if (ModelState.IsValid)
            {
                emprestimo.livro.ano = DateTime.Now;
                emprestimo.dataDevolucao = "26/04/2000 00:00:00";
                emprestimo.dataPrazo = Convert.ToString(dataPrazo);
                EmprestimoDAO.CadastrarEmprestimo(emprestimo);
                return RedirectToAction("Index");
            }

            return View(emprestimo);
        }


        // POST: Emprestimos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            EmprestimoDAO.RemoverEmprestimo(id);
            return RedirectToAction("Index");
        }
    }
}