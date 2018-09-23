using MundoNovo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MundoNovo.DAL
{
    public class EmprestimoDAO
    {

        private static Context ctx = Singleton.Instance.Context;

        public static List<Emprestimo> RetonarEmprestimos()
        {
            List<Emprestimo> listaEmprestimos = ctx.Emprestimos.ToList();
            foreach (Emprestimo item in listaEmprestimos)
            {
                DateTime dd = DateTime.Parse(item.dataDevolucao);
                DateTime dp = DateTime.Parse(item.dataPrazo);
                if (dd.Year == 2000)
                {
                    if (dp < DateTime.Now)
                    {
                        //atrasado
                        item.status = -1;
                    }
                    else
                    {
                        //pendente
                        item.status = 0;
                    }
                }
                else
                {
                    if (dd > dp)
                    {
                        //entregue com atraso
                        item.status = 2;
                    }
                    else
                    {
                        //entrega normal, no prazo
                        item.status = 1;
                    }

                }
            }

            return listaEmprestimos;
        }

        public static void EditarEmprestimo(Emprestimo emprestimo)
        {
            ctx.Entry(emprestimo).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public static void CadastrarEmprestimo(Emprestimo emprestimo)
        {

            ctx.Emprestimos.Add(emprestimo);
            ctx.SaveChanges();
        }

        public static Emprestimo BuscarEmprestimoPorId(int? id)
        {
            return ctx.Emprestimos.Find(id);
        }

        public static void RemoverEmprestimo(int id)
        {
            Emprestimo emprestimo = ctx.Emprestimos.Find(id);
            ctx.Emprestimos.Remove(emprestimo);
            ctx.SaveChanges();
        }
    }
}