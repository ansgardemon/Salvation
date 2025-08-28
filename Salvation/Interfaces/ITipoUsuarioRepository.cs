using Salvation.Models;

namespace Salvation.Interfaces
{
    public interface ITipoUsuarioRepository
    {

        Task<List<TipoUsuario>> GetAllAsync();

        //stand by
        Task<TipoUsuario> GetByIdAsyn(int id);
        Task AddAsync(TipoUsuario tipoUsuario);
        Task UpdateAsync(TipoUsuario tipoUsuario);
        Task DeleteAsync(int id);
    }
}
