using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using Salvation.Data;
using Salvation.ViewModels;

namespace Salvation.Controllers
{
    public class DashboardController : Controller
    {
        private readonly SalvationDbContext _context;

        public DashboardController(SalvationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel();

            // cards
            vm.TotalFilmes = await _context.Filmes.CountAsync();
            vm.TotalGeneros = await _context.Generos.CountAsync();
            vm.TotalClassificacoes = await _context.Classificacoes.CountAsync();
            vm.TotalUsuarios = await _context.Usuarios.CountAsync();

            // Últimos 5 filmes
            vm.UltimosFilmes = await _context.Filmes
                .AsNoTracking()
                .OrderByDescending(f => f.IdFilme)
                .Include(f => f.Genero)
                .Include(f => f.Classificacao)
                .Select(f => new FilmeItem
                {
                    IdFilme = f.IdFilme,
                    Titulo = f.Titulo,
                    Genero = f.Genero != null ? f.Genero.DescricaoGenero : "",
                    Classificacao = f.Classificacao != null ? f.Classificacao.DescricaoClassificacao : "",
                    UrlImagem = f.UrlImagem ?? ""
                })
                .Take(5)
                .ToListAsync();

            // Top 5 gêneros por contagem de filmes
            vm.GenerosMaisPopulares = await _context.Generos
                .AsNoTracking()
                .Select(g => new GeneroCount
                {
                    IdGenero = g.IdGenero,
                    DescricaoGenero = g.DescricaoGenero,
                    QtdeFilmes = g.Filmes.Count
                })
                .OrderByDescending(x => x.QtdeFilmes)
                .ThenBy(x => x.DescricaoGenero)
                .Take(5)
                .ToListAsync();



            // Filmes por gênero (todos)
            vm.FilmesPorGenero = await _context.Generos
                .AsNoTracking()
                .Select(g => new GeneroCount
                {
                    IdGenero = g.IdGenero,
                    DescricaoGenero = g.DescricaoGenero,
                    QtdeFilmes = g.Filmes.Count
                })
                .OrderByDescending(x => x.QtdeFilmes)
                .ToListAsync();

            // Filmes por classificação (todos)
            vm.FilmesPorClassificacao = await _context.Classificacoes
                .AsNoTracking()
                .Select(c => new ClassificacaoCount
                {
                    IdClassificacao = c.IdClassificacao,
                    DescricaoClassificacao = c.DescricaoClassificacao,
                    QtdeFilmes = c.Filmes.Count
                })
                .OrderByDescending(x => x.QtdeFilmes)
                .ToListAsync();

            // Cobertura de imagem
            vm.FilmesComImagem = await _context.Filmes
                .CountAsync(f => !string.IsNullOrEmpty(f.UrlImagem) && f.UrlImagem != "/img/sem-imagem.png");  // ajuste o nome se for UrlImageFilme
            vm.FilmesSemImagem = vm.TotalFilmes - vm.FilmesComImagem;

            // (Já temos vm.FilmesPorClassificacao da etapa anterior;
            // se não tiver, traga igual abaixo)
            if (vm.FilmesPorClassificacao == null || vm.FilmesPorClassificacao.Count == 0)
            {
                vm.FilmesPorClassificacao = await _context.Classificacoes
                    .AsNoTracking()
                    .Select(c => new ClassificacaoCount
                    {
                        IdClassificacao = c.IdClassificacao,
                        DescricaoClassificacao = c.DescricaoClassificacao,
                        QtdeFilmes = c.Filmes.Count
                    })
                    .OrderByDescending(x => x.QtdeFilmes)
                    .ToListAsync();
            }



            return View(vm);
        }
    }
}
