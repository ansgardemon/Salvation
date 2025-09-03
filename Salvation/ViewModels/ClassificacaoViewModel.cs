using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Salvation.ViewModels
{
    public class ClassificacaoViewModel
    {
        public int IdClassificacao { get; set; }

        [DisplayName("Classificação")]
        public string DescricaoClassificacao { get; set; }

    }
}
