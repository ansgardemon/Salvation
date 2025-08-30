using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salvation.Interfaces;
using Salvation.Models;
using Salvation.ViewModels;

namespace Salvation.Controllers
{
    public class GeneroController : Controller
    {


        //INJEÇÃO DE DEPENDENCIAS
        private readonly IGeneroRepository _generoRepository;



        //CONSTRUTOR
        public GeneroController(IGeneroRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }


        //METODO DE APOIO

        private async Task<GeneroViewModel> CriarGeneroViewModel(GeneroViewModel? model = null)
        {
            var generos = await _generoRepository.GetAllAsync();
            return new GeneroViewModel
            {
                IdGenero = model?.IdGenero ?? 0,
                DescricaoGenero = model?.DescricaoGenero
            };

        }





        //LISTAR
        [Authorize(Roles = "Administrador, Gerente")]
        public async Task<IActionResult> Index(int? search)
        {
            var generos = await _generoRepository.GetAllAsync();
            generos = generos.OrderByDescending(c => c.IdGenero).ToList();

            ViewBag.Search = search;

            return View(generos);

        }




        //CREATE - GET
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await CriarGeneroViewModel();
            return View(viewModel);
        }



        //CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GeneroViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var genero = new Genero
                {
                    DescricaoGenero = viewModel.DescricaoGenero
                };

                await _generoRepository.AddAsync(genero);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarGeneroViewModel();
            return View(viewModel);
        }


        //EDIT - GET
        [Authorize(Roles = "Administrador,Gerente")]
        public async Task<IActionResult> Edit(int id)
        {
            var genero = await _generoRepository.GetByIdAsyn(id);
            if (genero == null)
            {
                return NotFound();
            }
            var viewModel = new GeneroViewModel
            {
                IdGenero = genero.IdGenero,
                DescricaoGenero = genero.DescricaoGenero
            };
            return View(viewModel);
        }


        //EDIT - POST

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GeneroViewModel viewModel)
        {
            if (id != viewModel.IdGenero)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                var genero = await _generoRepository.GetByIdAsyn(id);
                if (genero == null)
                {
                    return NotFound();
                }
                genero.DescricaoGenero = viewModel.DescricaoGenero;

                await _generoRepository.UpdateAsync(genero);
                return RedirectToAction(nameof(Index));
            }
            viewModel = await CriarGeneroViewModel();
            return View(viewModel);
        }



        //DELETE - GET
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var genero = await _generoRepository.GetByIdAsyn(id);
            if (genero == null)
            {
                return NotFound();
            }
            return View(genero);
        }



        //DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var genero = await _generoRepository.GetByIdAsyn(id);
            if (genero == null)
            {
                return NotFound();
            }
            await _generoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
