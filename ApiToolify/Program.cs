using ApiToolify.ChatHub;
using ApiToolify.Data.Contratos;
using ApiToolify.Data.Repositorios;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProyectoDSWToolify.Data.Contratos;
using ProyectoDSWToolify.Data.Repositorios;
using ProyectoDSWToolify.Models;

var builder = WebApplication.CreateBuilder(args);

// ?? CORS para permitir al MVC acceder a la API con cookies
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy.WithOrigins("https://localhost:7108") // URL de tu proyecto MVC
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Necesario para cookies
    });
});

// ?? Autenticaci�n por cookies (asegura que SameSite=None y HTTPS)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

// ?? Inyecciones de dependencias
builder.Services.AddScoped<ICrud<Proveedor>, ProveedorCrudRepository>();
builder.Services.AddScoped<ICrud<Distrito>, DistritoRepository>();
builder.Services.AddScoped<ICrud<Producto>, ProductoCrudRepository>();
builder.Services.AddScoped<ICategoria, CategoriaRepository>();
builder.Services.AddScoped<IProducto, ProductoRepository>();
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IVenta, VentaRepository>();
builder.Services.AddScoped<IUserAuth, UserAuthRepository>();
builder.Services.AddScoped<IGrafico, GraficoRepository>();
builder.Services.AddScoped<IEstadistica, VendedorEstadisticasRepository>();
builder.Services.AddScoped<IReporte, ReporteRepository>();
builder.Services.AddScoped<IMensaje, MensajeRepository>();

var app = builder.Build();

// ?? Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowWebApp");

app.UseAuthentication(); // ??? Necesario para usar claims y cookies
app.UseAuthorization();

app.MapControllers();

// ?? SignalR Hub
app.MapHub<ChatHub>("/chatHub");

app.Run();
