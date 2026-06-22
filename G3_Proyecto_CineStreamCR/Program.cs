// Program.cs
// Punto de entrada principal de la aplicación.
// Aquí se inicializan y configuran todos los servicios y el pipeline HTTP
// para finalmente levantar la aplicación web.


var builder = WebApplication.CreateBuilder(args);


// ====================== SERVICIOS ======================

// Agrega los servicios necesarios para trabajar con MVC
// (Controladores y Vistas)
builder.Services.AddControllersWithViews();
// Registrar servicios de la capa BLL (el registro manual fue cancelado por petición)


// ====================== CONSTRUCCIÓN DE LA APP ======================

// Construye la aplicación con todos los servicios registrados
var app = builder.Build();


// ====================== CONFIGURACIÓN DEL PIPELINE ======================

// Si NO estamos en ambiente de desarrollo (Producción)
if (!app.Environment.IsDevelopment())
{
    // Redirige a una página de error personalizada
    app.UseExceptionHandler("/Home/Error");

    // Habilita HSTS para mayor seguridad (HTTPS obligatorio)
    app.UseHsts();
}

// Fuerza las solicitudes HTTP a HTTPS
app.UseHttpsRedirection();

// Habilita el sistema de enrutamiento (Routing)
app.UseRouting();

// Habilita la autorización de usuarios
// (si se implementan Login, Roles, etc.)
app.UseAuthorization();


// ====================== ARCHIVOS ESTÁTICOS ======================

// Permite servir archivos estáticos como:
// CSS, JavaScript, imágenes, Bootstrap, etc.
app.MapStaticAssets();


// ====================== RUTA POR DEFECTO ======================

// Define la ruta principal del proyecto:
//
// HomeController -> controlador por defecto
// Index() -> acción por defecto
// id -> parámetro opcional
//
// Ejemplo:
// https://localhost:7000/
// va a ejecutar HomeController -> Index()
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


// ====================== EJECUCIÓN DE LA APLICACIÓN ======================

// Inicia y mantiene la aplicación en ejecución
app.Run();