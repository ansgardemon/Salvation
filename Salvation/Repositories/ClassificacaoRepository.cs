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
        public async Task AddAsync(Classificacao classificacao)
        {
            await _context.Classificacoes.AddAsync(classificacao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var classificacao = _context.Classificacoes.FirstOrDefault(c => c.IdClassificacao == id);
            if (classificacao != null)
            {
                _context.Classificacoes.Remove(classificacao);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Classificacao> GetByIdAsyn(int id)
        {
            return await _context.Classificacoes.FirstOrDefaultAsync(c => c.IdClassificacao == id);
        }

        public async Task UpdateAsync(Classificacao classificacao)
        {
            _context.Classificacoes.Update(classificacao);
            await _context.SaveChangesAsync();
        }
    }
}
