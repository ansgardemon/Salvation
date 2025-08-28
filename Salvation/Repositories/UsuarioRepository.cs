using Salvation.Interfaces;
using Salvation.Models;
using Salvation.Data;
using Microsoft.EntityFrameworkCore;

namespace Salvation.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SalvationDbContext _context;

        public UsuarioRepository(SalvationDbContext context)
        {
            _context = context;
        }


        //create
        public async Task AddAsync(Usuario usuario)
        {
            await _context.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        //read
        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.Include(u => u.TipoUsuario).ToListAsync();
        }


        //read ativos
        public async Task<List<Usuario>> GetAllAtivosAsync()
        {
            return await _context.Usuarios.Include(u => u.TipoUsuario).Where(u => u.Ativo).ToListAsync();
        }


        //Search by id
        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios.Include(u => u.TipoUsuario).FirstOrDefaultAsync(u => u.IdUsuario == id);
        }


        //Inativar (soft delete)
        public async Task InativarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if(usuario != null && usuario.Ativo)
            {
                usuario.Ativo = false;
                await _context.SaveChangesAsync();
            }
        }

        //Reativar
        public async Task ReativarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null && !usuario.Ativo)
            {
                usuario.Ativo = true;
                await _context.SaveChangesAsync();
            }
        }

        //Update
        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Update(usuario);
            await _context.SaveChangesAsync();

        }
        //Valida Login
        public async Task<Usuario>? ValidarLoginAsync(string email, string senha)
        {
            return await _context.Usuarios.Include(u => u.TipoUsuario).FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }

    }
}
