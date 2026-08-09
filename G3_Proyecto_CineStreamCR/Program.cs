using G3_Proyecto_CineStreamCR.DAL.Data;
using Microsoft.EntityFrameworkCore;

// Program.cs
// Punto de entrada principal de la aplicación.
// Aquí se registran los servicios y se configura el pipeline HTTP.

var builder = WebApplication.CreateBuilder(args);


// ====================== SERVICIOS ======================

// Agrega soporte para MVC:
// Controllers + Views.
builder.Services.AddControllersWithViews();


// ====================== BASE DE DATOS ======================

// Obtiene la cadena de conexión desde appsettings.json.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

// Registra ApplicationDbContext mediante inyección de dependencias
// utilizando SQLite.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));


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


// Habilita autorización.
// La autenticación y sesión se configurarán cuando se implemente Login.
app.UseAuthorization();


// ====================== ARCHIVOS ESTÁTICOS ======================

// Permite servir CSS, JavaScript, imágenes y otros recursos.
app.MapStaticAssets();


// ====================== RUTA POR DEFECTO ======================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


// ====================== EJECUCIÓN ======================

app.Run();