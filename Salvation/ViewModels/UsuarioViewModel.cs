using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Salvation.ViewModels
{
    public class UsuarioViewModel
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        [DisplayName("Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [DisplayName("Tipo de usuário")]
        public int TipoUsuarioId { get; set; }

        //COLEÇÃO PARA POPULAR O DROPDOWN LIST
        public IEnumerable<SelectListItem>? TiposUsuario { get; set; }
    }
}
