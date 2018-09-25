using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemaMundoNovo.Models
{
    [Table("Emprestimos")]
    public class Emprestimo
    {
        [Key]
        public int EmprestimoId { get; set; }


        public int LivroID { get; set; }
        public virtual Livro _Livro { get; set; }


        public int BibliotecarioID { get; set; }
        public virtual Bibliotecario _Bibliotecario { get; set; }

        [DataType(DataType.Currency)]
        public double Valor { get; set; }
        public string DataPrazo { get; set; }
        public string DataDevolucao { get; set; }
        public int Status { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Nome { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }

    }
}