using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Salvation.Interfaces;
using Salvation.Models;
using Salvation.ViewModels;

namespace Salvation.Controllers
{
    public class FilmeController : Controller
    {
        //CAMPOS DE APOIO PARA INJEÇÃO DE DEPENDENCIAS

        private readonly IFilmeRepository _filmeRepository;
        private readonly IGeneroRepository _generoRepository;
        private readonly IClassificacaoRepository _classificacaoRepository;


        //CONSTRUTOR
        public FilmeController(IFilmeRepository filmeRepository,
                              IGeneroRepository generoRepository,
                              IClassificacaoRepository classificacaoRepository)
        {
            _filmeRepository = filmeRepository;
            _generoRepository = generoRepository;
            _classificacaoRepository = classificacaoRepository;
        }



        //MÉTODO DE APOIO
        private async Task<FilmeViewModel> CriarFilmeViewModel(FilmeViewModel? model = null)
        {
            var generos = await _generoRepository.GetAllAsync();
            var classificacoes = await _classificacaoRepository.GetAllAsync();


            return new FilmeViewModel
            {
                IdFilme = model?.IdFilme ?? 0,
                TituloFilme = model?.TituloFilme,
                ProdutoraFilme = model?.ProdutoraFilme,
                UrlImagem = model?.UrlImagem,
                ImagemUpload = model?.ImagemUpload,
                ClassificacaoId = model?.ClassificacaoId ?? 0,
                GeneroId = model?.GeneroId ?? 0,

                Generos = generos.Select(g => new SelectListItem
                {
                    Value = g.IdGenero.ToString(),
                    Text = g.DescricaoGenero
                }),

                Classificacoes = classificacoes.Select(c => new SelectListItem
                {
                    Value = c.IdClassificacao.ToString(),
                    Text = c.DescricaoClassificacao
                })



            };
        }



        //INDEX
        [Authorize(Roles = "Administrador, Gerente, Outros")]
        public async Task<IActionResult> Index(int? generoId, string? search)
        {
            var filmes = await _filmeRepository.GetAllAsync();


            //filtro
            if (generoId.HasValue && generoId.Value > 0)
            {
                filmes = filmes.Where(f => f.GeneroId == generoId.Value).ToList();
            }


            //search
            if (!string.IsNullOrEmpty(search))
            {
                filmes = filmes.Where(f => f.Titulo.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            filmes = filmes.OrderByDescending(f => f.IdFilme).ToList();

            //componentes
            ViewBag.Generos = new SelectList(await _generoRepository.GetAllAsync(), "IdGenero", "DescricaoGenero");
            ViewBag.FiltroGeneroId = generoId;
            ViewBag.Search = search;


            return View(filmes);
        }




        //CREATE PUXA PAGINA
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarFilmeViewModel();
            return View(viewModel);
        }



        //CREATE ALTERA BANCO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string caminhoImagem;

                if (viewModel.ImagemUpload != null)
                {
                    var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImagemUpload.FileName);



                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", nomeArquivo);


                    //Criar a pasta se não existir
                    using var stream = new FileStream(caminho, FileMode.Create);
                    await viewModel.ImagemUpload.CopyToAsync(stream);


                    caminhoImagem = "/img/" + nomeArquivo;


                }

                else
                {
                    // imagem padrão
                    caminhoImagem = "/img/sem-imagem.png";
                }

                var filme = new Filme
                {
                    Titulo = viewModel.TituloFilme,
                    Produtora = viewModel.ProdutoraFilme,
                    UrlImagem = caminhoImagem,
                    ClassificacaoId = viewModel.ClassificacaoId,
                    GeneroId = viewModel.GeneroId
                };

                await _filmeRepository.AddAsync(filme);
                return RedirectToAction(nameof(Index));

            }
            viewModel = await CriarFilmeViewModel(viewModel);
            return View(viewModel);
        }



        //EDIT PUXA PAGINA
        [Authorize(Roles = "Administrador, Gerente")]

        public async Task<IActionResult> Edit(int id)
        {

            var filme = await _filmeRepository.GetByIdAsync(id);
            if (filme == null)
            {
                return NotFound();
            }

            var viewModel = new FilmeViewModel
            {
                IdFilme = filme.IdFilme,
                TituloFilme = filme.Titulo,
                ProdutoraFilme = filme.Produtora,
                UrlImagem = filme.UrlImagem,
                ClassificacaoId = filme.ClassificacaoId,
                GeneroId = filme.GeneroId,

                Generos =
                (await _generoRepository.GetAllAsync()).Select(g => new SelectListItem
                {
                    Value = g.IdGenero.ToString(),
                    Text = g.DescricaoGenero
                }
                ),

                Classificacoes =
                (await _classificacaoRepository.GetAllAsync()).Select(c => new SelectListItem
                {
                    Value = c.IdClassificacao.ToString(),
                    Text = c.DescricaoClassificacao
                }
                )

            };


            return View(viewModel);
        }

        //EDIT ALTERA BANCO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FilmeViewModel viewModel)
        {
            if (id != viewModel.IdFilme)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var filme = await _filmeRepository.GetByIdAsync(id);
                if (filme == null)
                {
                    return NotFound();
                }

                filme.Titulo = viewModel.TituloFilme;
                filme.Produtora = viewModel.ProdutoraFilme;
                filme.ClassificacaoId = viewModel.ClassificacaoId;
                filme.GeneroId = viewModel.GeneroId;
                //verifica se tem uma nova imagem

                if (viewModel.ImagemUpload != null)
                {
                    var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ImagemUpload.FileName);
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", nomeArquivo);

                    using var stream = new FileStream(caminho, FileMode.Create);
                    await viewModel.ImagemUpload.CopyToAsync(stream);

                    filme.UrlImagem = "/img/" + nomeArquivo;
                }
                else if (string.IsNullOrEmpty(filme.UrlImagem))
                {
                    // fallback para imagem padrão se não tiver nenhuma
                    filme.UrlImagem = "/img/sem-imagem.png";
                }


                await _filmeRepository.UpdateAsync(filme);
                return RedirectToAction(nameof(Index));



            }

            viewModel = await CriarFilmeViewModel(viewModel);
            return View(viewModel);


        }

        //DELETE PUXA PAGINA
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var filme = await _filmeRepository.GetByIdAsync(id);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        //DELETE ALTERA BANCO  
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _filmeRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
