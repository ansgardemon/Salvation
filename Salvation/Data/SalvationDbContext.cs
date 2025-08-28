using Microsoft.EntityFrameworkCore;
using Salvation.Models;

namespace Salvation.Data
{

    //CLASSE RESPONSÁVEL PELO CONTEXTO DA BASE DE DADOS E MAPEAMENTO DAS ENTIDADES(TABELAS)
    public class SalvationDbContext : DbContext
    {
        //construtor
        public SalvationDbContext(DbContextOptions<SalvationDbContext> options) : base(options) { }

        //propriedades DbSet representam nossas tabelas
        public DbSet<Classificacao> Classificacoes { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<TipoUsuario> TipoUsuarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        //metodo opcional
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configurações adicionais podem ser feitas aqui, se necessário
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().Property(u => u.Ativo).HasDefaultValue(true);
        }
    }
}

