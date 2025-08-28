using Salvation.Models;

namespace Salvation.Interfaces
{
    public interface IClassificacaoRepository
    {
        Task<List<Classificacao>> GetAllAsync();

        //stand by
        Task<Classificacao> GetByIdAsyn(int id);
        Task AddAsync(Classificacao classificacao);
        Task UpdateAsync(Classificacao classificacao);
        Task DeleteAsync(int id);

    }
}
