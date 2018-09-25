using Newtonsoft.Json;
using SistemaMundoNovo.Models;
using SistemaMundoNovo.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SistemaMundoNovo.Controllers
{
    public class EmprestimosController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Emprestimos
        public ActionResult Index()
        {
            ApplicationUser b = UsuarioUtils.RetornaUsuarioLogado();
            int idBibliotecarioLogado = b._Bibliotecario.BibliotecarioID;
            List<Emprestimo> listaEmprestimos = db.Emprestimos.Where(x => x.BibliotecarioID == idBibliotecarioLogado).ToList();

            foreach (Emprestimo item in listaEmprestimos)
            {
                DateTime dd = DateTime.Parse(item.DataDevolucao);
                DateTime dp = DateTime.Parse(item.DataPrazo);
                if (dd.Year == 2000)
                {
                    if (dp < DateTime.Now)
                    {
                        //atrasado
                        item.Status = -1;
                    }
                    else
                    {
                        //pendente
                        item.Status = 0;
                    }
                }
                else
                {
                    if (dd > dp)
                    {
                        //entregue com atraso
                        item.Status = 2;
                    }
                    else
                    {
                        //entrega normal, no prazo
                        item.Status = 1;
                    }

                }
            }

            return View(listaEmprestimos);

        }

        // GET: Emprestimos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emprestimo emprestimo = db.Emprestimos.Find(id);
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
            double novoValor = emprestimo.Valor;
            emprestimo = db.Emprestimos.Find(emprestimo.EmprestimoId);
            emprestimo.Valor = novoValor;
            emprestimo.Status = status;
            emprestimo.DataDevolucao = Convert.ToString(dataDevolucao);
            if (ModelState.IsValid)
            {
                db.Entry(emprestimo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emprestimo);
        }

        [HttpPost]
        public ActionResult PesquisarCep(Emprestimo emprestimo)
        {
            string url = "https://viacep.com.br/ws/" + emprestimo.Cep + "/json/";
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

                emprestimo.Endereco = emprestimo.Logradouro + " " + emprestimo.Localidade;
                emprestimo.Livro = aux.Livro;
                emprestimo.Nome = aux.Nome;
                emprestimo.Status = aux.Status;
                emprestimo.BibliotecarioID = aux.BibliotecarioID;
                emprestimo.DataDevolucao = aux.DataDevolucao;
                emprestimo.DataPrazo = aux.DataPrazo;
                emprestimo.EmprestimoId = aux.EmprestimoId;
                emprestimo.Cep = aux.Cep;
                emprestimo.Valor = aux.Valor;

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
            emprestimo.Livro = db.Livros.Find(id);
       
            return View(emprestimo);
        }

        // POST: Emprestimos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,livro,valor,cep,endereco")] Emprestimo emprestimo, string nome, DateTime dataPrazo)
        {
           /* ApplicationUser b = UsuarioUtils.RetornaUsuarioLogado();
          int idBibliotecarioLogado = b._Bibliotecario.BibliotecarioID;*/
          

            string url = "https://viacep.com.br/ws/" + emprestimo.Cep + "/json/";
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
                emprestimo.Endereco = aux.Logradouro + emprestimo.Endereco;
                emprestimo.Bairro = aux.Bairro;
                emprestimo.Localidade = aux.Localidade;
                emprestimo.Uf = aux.Uf;
                emprestimo.Logradouro = aux.Logradouro;
            }
            catch
            {
                emprestimo.Cep = "Cep Inválido";
            }

            emprestimo.Status = 0;
            emprestimo.Nome = nome;
            emprestimo.BibliotecarioID = UsuarioUtils.RetornaIdBibliotecarioLogado();

            if (ModelState.IsValid)
            {
               
                emprestimo._Livro.ano = DateTime.Now;
                emprestimo.DataDevolucao = "26/04/2000 00:00:00";
                emprestimo.DataPrazo = DateTime.Now.AddDays(5).ToString();
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emprestimo);
        }


        // POST: Emprestimos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Emprestimo emprestimo = db.Emprestimos.Find(id);
            db.Emprestimos.Remove(emprestimo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}