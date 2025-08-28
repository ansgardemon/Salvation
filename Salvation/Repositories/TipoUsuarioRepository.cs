using Microsoft.EntityFrameworkCore;
using Salvation.Data;
using Salvation.Interfaces;
using Salvation.Models;

namespace Salvation.Repositories
{

    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly SalvationDbContext _context;
        public TipoUsuarioRepository(SalvationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TipoUsuario>> GetAllAsync()
        {
            return await _context.TipoUsuarios.ToListAsync();
        }
        public Task AddAsync(TipoUsuario tipoUsuario)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<TipoUsuario> GetByIdAsyn(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TipoUsuario tipoUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
