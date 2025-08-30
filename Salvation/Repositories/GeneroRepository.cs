using Microsoft.EntityFrameworkCore;
using Salvation.Data;
using Salvation.Interfaces;
using Salvation.Models;

namespace Salvation.Repositories
{

    public class GeneroRepository : IGeneroRepository
    {
        private readonly SalvationDbContext _context;
        public GeneroRepository(SalvationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Genero>> GetAllAsync()
        {
            return await _context.Generos.ToListAsync();
        }
        public async Task AddAsync(Genero genero)
        {
            await _context.Generos.AddAsync(genero);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var genero = _context.Generos.FirstOrDefault(g => g.IdGenero == id);
            if (genero != null)
            {
                _context.Generos.Remove(genero);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Genero> GetByIdAsyn(int id)
        {
            return await _context.Generos.FirstOrDefaultAsync(g => g.IdGenero == id);
        }

        public async Task UpdateAsync(Genero genero)
        {
            _context.Generos.Update(genero);
            await _context.SaveChangesAsync();
        }
    }
}
