using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salvation.Interfaces;
using Salvation.Models;
using Salvation.Repositories;
using Salvation.ViewModels;

namespace Salvation.Controllers
{
    public class ClassificacaoController : Controller
    {


        //INJEÇÃO DE DEPENDENCIAS
        private readonly IClassificacaoRepository _classificacaoRepository;



        //CONSTRUTOR
        public ClassificacaoController(IClassificacaoRepository classificacaoRepository)
        {
            _classificacaoRepository = classificacaoRepository;
        }


        //METODO DE APOIO

        private async Task<ClassificacaoViewModel> CriarClassificacaoViewModel(ClassificacaoViewModel? model = null)
        {
            var classificacoes = await _classificacaoRepository.GetAllAsync();
            return new ClassificacaoViewModel
            {
                IdClassificacao = model?.IdClassificacao ?? 0,
                DescricaoClassificacao = model?.DescricaoClassificacao
            };

        }





        //LISTAR
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Index(int? search)
        {
            var classificacoes = await _classificacaoRepository.GetAllAsync();
            classificacoes = classificacoes.OrderByDescending(c => c.IdClassificacao).ToList();

            ViewBag.Search = search;

            return View(classificacoes);

        }




        //CREATE - GET
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarClassificacaoViewModel();
            return View(viewModel);
        }



        //CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassificacaoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var classificacao = new Classificacao
                {
                    DescricaoClassificacao = viewModel.DescricaoClassificacao
                };

                await _classificacaoRepository.AddAsync(classificacao);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarClassificacaoViewModel();
            return View(viewModel);
        }


        //EDIT - GET
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            var classificacao = await _classificacaoRepository.GetByIdAsyn(id);
            if (classificacao == null)
            {
                return NotFound();
            }
            var viewModel = new ClassificacaoViewModel
            {
                IdClassificacao = classificacao.IdClassificacao,
                DescricaoClassificacao = classificacao.DescricaoClassificacao
            };
            return View(viewModel);
        }


        //EDIT - POST

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassificacaoViewModel viewModel)
        {
            if (id != viewModel.IdClassificacao)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                var classificacao = await _classificacaoRepository.GetByIdAsyn(id);
                if (classificacao == null)
                {
                    return NotFound();
                }
                classificacao.DescricaoClassificacao = viewModel.DescricaoClassificacao;

                await _classificacaoRepository.UpdateAsync(classificacao);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarClassificacaoViewModel();
            return View(viewModel);
        }



        //DELETE - GET
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var classificacao = await _classificacaoRepository.GetByIdAsyn(id);
            if (classificacao == null)
            {
                return NotFound();
            }
            return View(classificacao);
        }



        //DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var classificacao = await _classificacaoRepository.GetByIdAsyn(id);
            if (classificacao == null)
            {
                return NotFound();
            }
            await _classificacaoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
