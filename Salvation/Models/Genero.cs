using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Salvation.Models
{
    [Table("Genero")]
    public class Genero
    {
        [Key]
        public int IdGenero { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [Display(Name = "Gênero")]
        [StringLength(150)]
        public string DescricaoGenero { get; set; }

        public List<Filme> Filmes { get; set; } = new List<Filme>();
    }
}
