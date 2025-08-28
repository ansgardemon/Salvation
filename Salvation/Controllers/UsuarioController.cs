using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Salvation.Interfaces;
using Salvation.ViewModels;
using Salvation.Models;

namespace Salvation.Controllers;

public class UsuarioController : Controller
{
    //campo de apoio
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITipoUsuarioRepository _tipoUsuarioRepository;


    //Metodo de apoio

    private async Task<UsuarioViewModel> CriarUsuarioViewModel(UsuarioViewModel? model = null)
    {
        var tipos = await _tipoUsuarioRepository.GetAllAsync();

        return new UsuarioViewModel
        {
            IdUsuario = model?.IdUsuario ?? 0,
            Nome = model?.Nome,
            Email = model?.Email,
            Senha = model?.Senha,
            DataNascimento = model?.DataNascimento ?? DateTime.Now,
            TipoUsuarioId = model?.TipoUsuarioId ?? 0,
            TiposUsuario = tipos.Select(t => new SelectListItem
            {
                Value = t.IdTipoUsuario.ToString(),
                Text = t.DescricaoTipoUsuario
            })
        };
    }


    //construtor
    public UsuarioController(
        IUsuarioRepository usuarioRepository,
        ITipoUsuarioRepository tipoUsuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
        _tipoUsuarioRepository = tipoUsuarioRepository;
    }


    //index
    [Authorize(Roles = "Administrador,Gerente,Outros")]
    public async Task<IActionResult> Index(int? tipoUsuarioId, string? search)
    {
        var usuarios = await _usuarioRepository.GetAllAtivosAsync();

        if (tipoUsuarioId.HasValue && tipoUsuarioId.Value > 0)
            usuarios = usuarios.Where(u => u.TipoUsuarioId == tipoUsuarioId).ToList();

        if (!string.IsNullOrEmpty(search))
            usuarios = usuarios.Where(u => u.Nome.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

        usuarios = usuarios.OrderByDescending(u => u.IdUsuario).ToList();

        ViewBag.TiposUsuario = new SelectList(await _tipoUsuarioRepository.GetAllAsync(), "IdTipoUsuario", "DescricaoTipoUsuario");
        ViewBag.FiltroTipoId = tipoUsuarioId;
        ViewBag.TermoBusca = search;

        return View(usuarios);
    }


    //login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string senha)
    {
        var usuario = await _usuarioRepository.ValidarLoginAsync(email, senha);
        if (usuario == null || !usuario.Ativo)
        {
            ModelState.AddModelError("", "Usuário ou senha inválidos.");
            return View();
        }

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim(ClaimTypes.Role, usuario.TipoUsuario.DescricaoTipoUsuario)
    };

        var identity = new ClaimsIdentity(claims, "SaphiraAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("SaphiraAuth", principal);

        return RedirectToAction("Index", "Home");
    }


    //logout
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("SaphiraAuth");
        return RedirectToAction("Login");
    }
    //acesso negado
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AcessoNegado()
    {
        return View();
    }

    //create
    [Authorize(Roles = "Administrador,Gerente")]
    public async Task<IActionResult> Create()
    {
        var viewModel = await CriarUsuarioViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var usuario = new Usuario
            {
                Nome = viewModel.Nome,
                Email = viewModel.Email,
                Senha = viewModel.Senha,
                DataNascimento = viewModel.DataNascimento,
                TipoUsuarioId = viewModel.TipoUsuarioId,
                Ativo = true
            };

            await _usuarioRepository.AddAsync(usuario);
            return RedirectToAction(nameof(Index));
        }

        viewModel = await CriarUsuarioViewModel(viewModel);
        return View(viewModel);
    }





    //EDIT
    [Authorize(Roles = "Administrador,Gerente")]
    public async Task<IActionResult> Edit(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        var viewModel = new UsuarioViewModel
        {
            IdUsuario = usuario.IdUsuario,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Senha = usuario.Senha,
            DataNascimento = usuario.DataNascimento,
            TipoUsuarioId = usuario.TipoUsuarioId,
            TiposUsuario =
            (
                await _tipoUsuarioRepository.GetAllAsync()).Select(t => new SelectListItem
                {
                    Value = t.IdTipoUsuario.ToString(),
                    Text = t.DescricaoTipoUsuario
                }).ToList()

        };

        return View(viewModel);
    }

    //EDIT ALTERA BANCO DE DADOS


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UsuarioViewModel viewModel)
    {

        if (id != viewModel.IdUsuario)
        {
            return NotFound();
        }


        if (ModelState.IsValid)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            usuario.Nome = viewModel.Nome;
            usuario.Email = viewModel.Email;
            usuario.Senha = viewModel.Senha;
            usuario.DataNascimento = viewModel.DataNascimento;
            usuario.TipoUsuarioId = viewModel.TipoUsuarioId;


            await _usuarioRepository.UpdateAsync(usuario);
            return RedirectToAction(nameof(Index));
        }
        viewModel = await CriarUsuarioViewModel(viewModel);
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                Console.WriteLine($"Erro em {state.Key}: {error.ErrorMessage}");
            }
        }

        return View(viewModel);
    }


    //Soft Delete
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View(usuario);
    }




    //Soft Delete Altera Banco
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        await _usuarioRepository.InativarAsync(id);
        return RedirectToAction(nameof(Index));
    }

    //Listar Inativos
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Inativos()
    {
        var usuarios = await _usuarioRepository.GetAllAsync();

        var inativos = usuarios
            .Where(u => !u.Ativo)
            .OrderByDescending(u => u.IdUsuario)
            .ToList();

        return View(inativos);
            
    }



    //Reativar

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AtivarUsuario(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View(usuario);
    }



    //Reativar Altera Banco
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AtivarUsuarioConfirm(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        await _usuarioRepository.ReativarAsync(id);
        return RedirectToAction(nameof(Index));
    }



    
}
