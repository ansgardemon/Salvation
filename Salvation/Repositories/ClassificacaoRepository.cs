using Microsoft.EntityFrameworkCore;
using Salvation.Data;
using Salvation.Interfaces;
using Salvation.Models;

namespace Salvation.Repositories
{
    public class ClassificacaoRepository : IClassificacaoRepository
    {
        private readonly SalvationDbContext _context;
        public ClassificacaoRepository(SalvationDbContext context)
        {
            _context = context;
        }
        //implementar apenas esse método
        public async Task<List<Classificacao>> GetAllAsync()
        {
            return await _context.Classificacoes.ToListAsync();
        }
        public Task AddAsync(Classificacao classificacao)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<Classificacao> GetByIdAsyn(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Classificacao classificacao)
        {
            throw new NotImplementedException();
        }
    }
}
