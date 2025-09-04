using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Salvation.Models;
using Salvation.Interfaces;

namespace Salvation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFilmeRepository _filmeRepository;

        public HomeController(ILogger<HomeController> logger,
                              IFilmeRepository filmeRepository)
        {
            _logger = logger;
            _filmeRepository = filmeRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Busca todos os filmes do banco
            var filmes = await _filmeRepository.GetAllAsync();
            filmes = filmes.OrderByDescending(f => f.IdFilme).ToList(); // Últimos lançamentos primeiro
            return View(filmes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
