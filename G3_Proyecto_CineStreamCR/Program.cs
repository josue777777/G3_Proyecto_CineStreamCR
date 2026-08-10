using G3_Proyecto_CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;
using G3_Proyecto_CineStreamCR.BLL.Services.Usuario;
using G3_Proyecto_CineStreamCR.DAL.Repositorios.Usuario;


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

// Los repositorios y servicios BLL se registrarán posteriormente
// conforme se implemente cada módulo.

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
app.MapStaticAssets();

// ====================== RUTA POR DEFECTO ======================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();

// ====================== EJECUCIÓN ======================

app.Run();