using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMundoNovo.Models
{
    [Table("Livros")]
    public class Livro
    {

        [Key]
        public int id { get; set; }

        [Display(Name = "Titulo")]
        public string titulo { get; set; }

        [Display(Name = "Autor")]
        public string autor { get; set; }

        [Display(Name = "Data dd/mm/aa")]
        [DisplayFormat(DataFormatString = "dd/mm/aa")]
        public DateTime ano { get; set; }

        [Display(Name = "Categoria")]
        public Categoria categoria { get; set; }

        [Display(Name = "Descrição")]
        public string descricao { get; set; }

        // relacionamento 1-1
        public int BibliotecarioID { get; set; }
        public virtual Bibliotecario _Bibliotecario { get; set; }


    }
}