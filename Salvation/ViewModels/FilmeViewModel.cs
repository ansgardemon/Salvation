using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Salvation.ViewModels
{
    public class FilmeViewModel
    {
        public int IdFilme { get; set; }

        [DisplayName("Título")]
        public string TituloFilme { get; set; }

        [DisplayName("Produtora")]
        public string ProdutoraFilme { get; set; }
        public string? UrlImagem { get; set; }

        [DisplayName("Capa")]
        public IFormFile? ImagemUpload { get; set; }

        [DisplayName("Classificação Etária")]
        public int ClassificacaoId { get; set; }


        [DisplayName("Gênero")]
        public int GeneroId { get; set; }



        //COLEÇÃO PARA POPULAR O DROPDOWN LIST

        public IEnumerable<SelectListItem>? Classificacoes { get; set; }
        public IEnumerable<SelectListItem>? Generos { get; set; }
    }
}
