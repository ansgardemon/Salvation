using Salvation.Models;

namespace Salvation.Interfaces
{
    public interface IFilmeRepository
    {
        Task<List<Filme>> GetAllAsync();

        //stand by
        Task<Filme> GetByIdAsync(int id);
        Task AddAsync(Filme filme);
        Task UpdateAsync(Filme filme);
        Task DeleteAsync(int id);


    }
}
