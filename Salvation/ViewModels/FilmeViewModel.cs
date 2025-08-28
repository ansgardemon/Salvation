using Microsoft.AspNetCore.Mvc.Rendering;

namespace Salvation.ViewModels
{
    public class FilmeViewModel
    {
        public int IdFilme { get; set; }
        public string TituloFilme { get; set; }
        public string ProdutoraFilme { get; set; }
        public string? UrlImagem { get; set; }
        public IFormFile? ImagemUpload { get; set; }
        public int ClassificacaoId { get; set; }
        public int GeneroId { get; set; }



        //COLEÇÃO PARA POPULAR O DROPDOWN LIST

        public IEnumerable<SelectListItem>? Classificacoes { get; set; }
        public IEnumerable<SelectListItem>? Generos { get; set; }
    }
}
