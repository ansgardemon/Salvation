using System.ComponentModel;

namespace Salvation.ViewModels
{
    public class GeneroViewModel
    {
        public int IdGenero { get; set; }

        [DisplayName("Gênero")]
        public string DescricaoGenero { get; set; }

    }
}
