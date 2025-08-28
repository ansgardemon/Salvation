using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Salvation.Models
{
    [Table("Filme")]
    public class Filme
    {
        [Key]
        public int IdFilme { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [Display(Name = "Título")]
        [StringLength(150)]
        public string Titulo { get; set; }


        [Required(ErrorMessage = "Campo obrigatório!")]
        [Display(Name = "Produtora")]
        [StringLength(150)]
        public string Produtora { get; set; }


        [Required(ErrorMessage = "Campo obrigatório!")]
        [Display(Name = "Imagem")]
        [StringLength(150)]
        public string UrlImagem { get; set; }

        //propriedades de relacionamento

        public int ClassificacaoId { get; set; }
        public virtual Classificacao? Classificacao { get; set; }

        public int GeneroId { get; set; }
        public virtual Genero? Genero { get; set; }




    }
}
