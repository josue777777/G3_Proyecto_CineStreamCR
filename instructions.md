# Buenas prácticas (resumen)

Este documento recoge recomendaciones prácticas para trabajar con este proyecto ASP.NET Core + EF Core (SQLite).

- Configuración y secretos
  - Mantener la cadena de conexión fuera del código fuente. Usar `appsettings.Development.json` y/o _user secrets_ (`dotnet user-secrets`) para entorno local.
  - En producción usar variables de entorno o un servicio de secretos (por ejemplo Azure Key Vault).

- Entity Framework Core
  - Registrar el `DbContext` vía DI(Inyección de dependencias) (ya está en `Program.cs`).
  - No hardcodear la cadena en `OnConfiguring`; permitir que DI la inyecte.
  - Usar migraciones (`dotnet ef migrations add <Nombre>` / `dotnet ef database update`) para evolucionar el esquema. Para SQLite, revisar limitaciones de alteraciones.
  - Mantener las entidades en la carpeta `Entidades` y el contexto en `Data`.

- Código y estilo
  - Habilitar `nullable` (ya está en el proyecto). Manejar referencias nulas explícitamente y usar tipos anulables cuando corresponda.
  - Preferir métodos `async` para acceso a datos (`ToListAsync`, `SaveChangesAsync`).
  - Seguir convenciones PascalCase para clases y propiedades.

- Control de versiones y PRs
  - Hacer commits pequeños y con mensajes claros. Abrir PRs para cambios significativos y pedir revisión.
  - Añadir un `.gitignore` apropiado y no commitear binarios, secretos ni bases de datos locales.

- Comandos útiles
  - Paquetes EF Core:
    - `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`
    - `dotnet add package Microsoft.EntityFrameworkCore.Design`
  - Herramienta CLI: `dotnet tool install --global dotnet-ef`
  - Scaffold desde SQLite (genera entidades y contexto):
    - `dotnet ef dbcontext scaffold "Data Source=C:\\ruta\\a\\tu.db" Microsoft.EntityFrameworkCore.Sqlite --output-dir Entidades --context ApplicationDbContext --context-dir Data --force`
  - Migraciones:
    - `dotnet ef migrations add InitialCreate`
    - `dotnet ef database update`

- Otras recomendaciones
  - Documentar cómo ejecutar el proyecto en desarrollo (`dotnet restore`, `dotnet build`, `dotnet run`).
  - Mantener dependencias actualizadas y planear actualizaciones mayores con pruebas.

Si quieres, puedo añadir ejemplos concretos de `appsettings.json`, plantillas de pruebas o eliminar la clase placeholder `Ejemplo.cs` generada por el scaffold.