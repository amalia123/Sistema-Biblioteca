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

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime ano { get; set; }

        [Display(Name = "Categoria")]
        public int categoriaId { get; set; }
        public virtual Categoria _categoria { get; set; }

        [Display(Name = "Descrição")]
        public string descricao { get; set; }

        // relacionamento 1-1
        public int BibliotecarioID { get; set; }
        public virtual Bibliotecario _Bibliotecario { get; set; }


    }
}