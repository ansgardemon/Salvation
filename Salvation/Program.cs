using Microsoft.EntityFrameworkCore;
using Salvation.Data;
using Salvation.Interfaces;
using Salvation.Repositories;

var builder = WebApplication.CreateBuilder(args);





//Base de Dados
builder.Services.AddDbContext<SalvationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// ======================================================
// Registro dos REPOSITÓRIOS (injeção de dependência)
// ======================================================
// Cada interface é ligada à sua implementação concreta.
// AddScoped ? ciclo de vida: uma instância por requisição HTTP.
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IGeneroRepository, GeneroRepository>();
builder.Services.AddScoped<IClassificacaoRepository, ClassificacaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();

// ======================================================
// Autenticação com COOKIES
// ======================================================
// Define "SaphiraAuth" como esquema de autenticação padrão.
// - LoginPath: rota para login quando usuário não autenticado.
// - AccessDeniedPath: rota de acesso negado para roles.
// - ExpireTimeSpan: duração do cookie de autenticação.
// - SlidingExpiration: renova o cookie se o usuário continuar ativo.
builder.Services.AddAuthentication("SaphiraAuth")
    .AddCookie("SaphiraAuth", options =>
    {
        options.LoginPath = "/Usuario/Login";             // Página de login
        options.AccessDeniedPath = "/Usuario/AcessoNegado"; // Página de acesso negado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Expira em 30 min
        options.SlidingExpiration = true;                  // Renovação automática
    });





// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



//Repositories





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
