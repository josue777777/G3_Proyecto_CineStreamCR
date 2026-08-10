using G3_Proyecto_CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;
using G3_Proyecto_CineStreamCR.BLL.Services.Usuario;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.Usuario;
using G3_Proyecto_CineStreamCR.BLL.Services.Calificacion;
using G3_Proyecto_CineStreamCR.BLL.Services.Reproduccion;
using G3_Proyecto_CineStreamCR.BLL.Services.Pelicula;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.Pelicula;
using G3_Proyecto_CineStreamCR.BLL.Services.WatchList;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.WatchList;
using G3_Proyecto_CineStreamCR.BLL.Services.Persona;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.Persona;
using G3_Proyecto_CineStreamCR.Data;


// Program.cs
// Punto de entrada principal de la aplicación.
// Aquí se registran los servicios y se configura el pipeline HTTP.

var builder = WebApplication.CreateBuilder(args);

// ====================== SERVICIOS ======================

// Agrega soporte para MVC:
// Controllers + Views.
builder.Services.AddControllersWithViews();

// ====================== SESIÓN ======================

// Permite mantener información básica del usuario
// mientras navega por la aplicación.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ====================== BASE DE DATOS ======================

// Obtiene la cadena de conexión desde appsettings.json.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

// Registra ApplicationDbContext mediante inyección de dependencias
// utilizando SQLite.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// ====================== DEPENDENCIAS DE USUARIO ======================

// Repositorio de acceso a datos de usuarios.
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// Servicio de lógica de negocio para autenticación y usuarios.
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();

// Registra dependencias de calificación y reproducción.
builder.Services.AddScoped<ICalificacionService, CalificacionService>();
builder.Services.AddScoped<IReproduccionService, ReproduccionService>();

// ====================== DEPENDENCIAS DE CATÁLOGO / DETALLE (PELÍCULA) ======================

// Catálogo y detalle de películas.
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IPeliculaService, PeliculaService>();

// Integración mínima de WatchList reutilizada por el catálogo y el detalle.
builder.Services.AddScoped<IWatchListRepositorio, WatchListRepositorio>();
builder.Services.AddScoped<IWatchListService, WatchListService>();

// Perfil público de personas (director/actor), usado desde el detalle.
builder.Services.AddScoped<IPersonaRepositorio, PersonaRepositorio>();
builder.Services.AddScoped<IPersonaService, PersonaService>();

// ====================== CONSTRUCCIÓN DE LA APP ======================

var app = builder.Build();



// ====================== PIPELINE HTTP ======================

if (!app.Environment.IsDevelopment())
{
    // Manejo de errores en ambientes diferentes a desarrollo.
    app.UseExceptionHandler("/Home/Error");

    // Fuerza políticas de seguridad HTTPS.
    app.UseHsts();
}

// Redirección HTTP -> HTTPS.
app.UseHttpsRedirection();

// Habilita enrutamiento.
app.UseRouting();

// Habilita el uso de sesión antes de procesar
// la autorización y los controladores.
app.UseSession();

// Habilita autorización.
// La autenticación se completará conforme avance el módulo de Login.
app.UseAuthorization();

// ====================== ARCHIVOS ESTÁTICOS ======================

// Permite servir CSS, JavaScript, imágenes y otros recursos.
app.UseStaticFiles();
// ====================== RUTA POR DEFECTO ======================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ====================== EJECUCIÓN ======================

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.SeedAsync(dbContext);
}

app.Run();