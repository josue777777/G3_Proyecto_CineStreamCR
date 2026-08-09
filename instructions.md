# CineStream CR - Instrucciones y buenas prácticas

Este documento define las reglas de desarrollo para el Proyecto Final
"CineStream CR" del curso SC-701 Programación Avanzada.

El objetivo es mantener una arquitectura sencilla, clara, mantenible y
alineada con los contenidos vistos en clase y con el enunciado oficial.

Antes de realizar cualquier modificación en el proyecto, revisar estas
instrucciones y respetar la arquitectura existente.


# 1. Alcance del proyecto

CineStream CR es una plataforma web de streaming de películas inspirada
visualmente en plataformas como Netflix, Disney+, HBO Max o Prime Video.

Implementar únicamente las funcionalidades solicitadas en el enunciado:

- Login.
- Catálogo de películas.
- Búsqueda, filtros, paginación y ordenamiento.
- Detalle de películas.
- Información de directores y actores.
- WatchLists.
- Calificaciones y reseñas.
- Reproducción de películas.
- Mini-reproductor persistente.
- Notificaciones visuales.
- Indicadores de carga.
- Diseño responsive.
- Tema oscuro.

No agregar funcionalidades no solicitadas como:

- Suscripciones.
- Pagos.
- Planes premium.
- Recomendaciones mediante IA.
- Chats.
- Redes sociales.
- Funcionalidades administrativas adicionales.

Si una nueva funcionalidad no aparece en el enunciado, no implementarla
sin autorización previa.


# 2. Arquitectura

Mantener la arquitectura actual por capas:

G3_Proyecto_CineStreamCR
    -> Proyecto ASP.NET Core MVC.
    -> Interfaz de usuario.
    -> Controllers, Views, ViewModels y recursos frontend.

G3_Proyecto_CineStreamCR.BLL
    -> Capa de lógica de negocio.
    -> DTOs.
    -> Servicios.
    -> Interfaces de servicios.
    -> AutoMapper.

G3_Proyecto_CineStreamCR.DAL
    -> Capa de acceso a datos.
    -> Entidades.
    -> Repositorios.
    -> Interfaces de repositorios.
    -> ApplicationDbContext.
    -> Migraciones.
    -> SQLite.

Flujo principal:

MVC
 -> BLL / Services
 -> DAL / Repositories
 -> Entity Framework Core
 -> SQLite

No acceder directamente a ApplicationDbContext desde los Controllers.

No acceder directamente a repositorios desde las Views.

No colocar lógica de negocio en las Views.


# 3. API

Actualmente una API independiente NO es requisito obligatorio para
la arquitectura base del proyecto.

No crear un proyecto Web API adicional salvo que una funcionalidad
realmente lo requiera y haya sido previamente autorizada.

Cuando sea posible resolver una funcionalidad manteniendo MVC + BLL + DAL,
preferir la arquitectura existente.

Si posteriormente se requiere una API para funcionalidades dinámicas,
debe reutilizar la BLL existente y no duplicar lógica de negocio.


# 4. Organización de carpetas

DAL:

Entidades/
    Usuario.cs
    Persona.cs
    Genero.cs
    Pelicula.cs
    PeliculaGenero.cs
    PeliculaActor.cs
    WatchList.cs
    WatchListPelicula.cs
    Calificacion.cs
    Reproduccion.cs

Repositorios/
    Usuario/
    Persona/
    Pelicula/
    WatchList/
    Calificacion/
    Reproduccion/

Data/
    ApplicationDbContext.cs


BLL:

Dtos/
    Usuarios/
    Personas/
    Peliculas/
    WatchLists/
    Calificaciones/
    Reproducciones/

Services/
    Usuario/
    Persona/
    Pelicula/
    WatchList/
    Calificacion/
    Reproduccion/

MapeoClases.cs


MVC:

Controllers/
Models/
ViewModels/
Views/
wwwroot/


# 5. Entity Framework Core

Utilizar SQLite mediante Entity Framework Core.

Registrar ApplicationDbContext mediante inyección de dependencias.

No hardcodear la cadena de conexión dentro de ApplicationDbContext.

La cadena debe obtenerse desde configuración, por ejemplo:

appsettings.json
appsettings.Development.json
User Secrets
variables de entorno.

Utilizar migraciones para modificar el esquema:

Add-Migration NombreMigracion
Update-Database

No modificar manualmente migraciones que ya hayan sido aplicadas.

ApplicationDbContext debe permanecer en:

G3_Proyecto_CineStreamCR.DAL/Data

# Clarificaciones adicionales sobre Entity Framework Core

- Las entidades pertenecen al DAL y deben definirse dentro de G3_Proyecto_CineStreamCR.DAL/Entidades.
- Las relaciones entre entidades se configuran principalmente mediante Fluent API en ApplicationDbContext.
- Las propiedades de navegación representan relaciones entre entidades y deben diseñarse para reflejar correctamente esas relaciones.
- EF Core será la fuente real del esquema mediante migraciones; cualquier script SQL (por ejemplo script.sql) funciona únicamente como documentación del esquema esperado.

Control de paquetes NuGet (regla breve)

- Mantener los paquetes compatibles con .NET 8.
- Evitar mezclar versiones mayores de Entity Framework Core.
- Los paquetes Microsoft.EntityFrameworkCore.* utilizados conjuntamente deben mantenerse en versiones compatibles entre sí.
- Revisar advertencias NU19xx de vulnerabilidades antes de entregar.
- No actualizar paquetes automáticamente a una versión mayor solo porque NuGet indique que existe una actualización.
- Una actualización de paquetes debe realizarse de forma controlada, recompilando y probando el proyecto después del cambio.


# 6. Consultas y tracking

Utilizar métodos async para operaciones de acceso a datos:

ToListAsync()
FirstOrDefaultAsync()
AnyAsync()
SaveChangesAsync()

Utilizar IQueryable cuando permita construir consultas dinámicas,
especialmente para:

- búsqueda por título;
- filtros por género;
- filtros por año;
- ordenamiento;
- paginación.

Usar AsNoTracking() en consultas exclusivamente de lectura.

Ejemplos:

- catálogo;
- detalle únicamente para mostrar;
- perfiles;
- búsquedas.

Mantener tracking cuando la entidad obtenida vaya a ser modificada
o eliminada.

No utilizar "Astracking".

EF Core realiza tracking por defecto.

AsTracking() únicamente debe utilizarse explícitamente cuando sea
necesario dejar clara esa intención.


# 7. Repositorios

Los repositorios son responsables únicamente del acceso a datos.

No colocar reglas de negocio en los repositorios.

Ejemplos de responsabilidades válidas:

- obtener películas;
- buscar por Id;
- consultar WatchLists;
- agregar entidades;
- actualizar entidades;
- eliminar entidades;
- guardar cambios.

No utilizar listas estáticas o datos simulados en memoria cuando exista
un repositorio real conectado a Entity Framework Core.


# 8. Servicios BLL

Los servicios contienen las reglas de negocio.

Ejemplos:

- validar login;
- crear y modificar WatchLists;
- impedir duplicados;
- validar calificaciones entre 1 y 10;
- actualizar progreso de reproducción;
- decidir si una película ya pertenece a una WatchList.

Los Controllers deben delegar estas operaciones a los servicios.


# 9. DTOs

Las entidades de DAL representan la base de datos.

Los DTOs representan la información que se mueve entre capas.

No utilizar las entidades directamente como modelos de las Views cuando
un DTO o ViewModel sea más apropiado.

Organizar DTOs por módulo.

Ejemplo:

Dtos/Peliculas/PeliculaDto.cs
Dtos/Usuarios/UsuarioDto.cs
Dtos/WatchLists/WatchListDto.cs

Evitar múltiples DTOs innecesarios para una misma entidad.

Crear DTOs adicionales únicamente cuando exista una diferencia real
entre las operaciones.


# 10. AutoMapper

Centralizar los mapeos en:

BLL/MapeoClases.cs

Ejemplo:

CreateMap<Pelicula, PeliculaDto>().ReverseMap();

Agregar configuraciones específicas únicamente cuando las propiedades
no coincidan directamente.

No realizar mapeos manuales repetitivos si AutoMapper ya puede resolverlos.


# 11. Validaciones

Aplicar validaciones tanto en cliente como en servidor cuando corresponda.

Utilizar DataAnnotations para validaciones básicas:

[Required]
[StringLength]
[Range]
[EmailAddress]

Nunca confiar únicamente en validaciones de JavaScript.

Las reglas importantes también deben validarse en la BLL.


# 12. Seguridad del login

Nunca almacenar contraseñas en texto plano.

Guardar únicamente un hash seguro de la contraseña.

La entidad Usuario debe utilizar una propiedad similar a:

PasswordHash

No devolver PasswordHash en DTOs ni mostrarlo en Views.

Los mensajes de login deben ser descriptivos para el usuario según los
requisitos del proyecto.


# 13. Modelo de personas

Utilizar una entidad Persona para representar directores y actores.

No crear entidades separadas Director y Actor salvo que aparezca una
necesidad real que lo justifique.

Una película mantiene su director mediante IdDirector.

Los actores se relacionan mediante PeliculaActor.

PeliculaActor debe almacenar también el personaje interpretado.


# 14. Calificaciones y reseñas

Mantener puntuación y reseña opcional en el mismo modelo de interacción.

La puntuación debe estar entre 1 y 10.

Un usuario debe mantener como máximo una calificación por película.

Si vuelve a calificar, actualizar la existente en lugar de crear
registros duplicados.


# 15. WatchLists

Cada WatchList pertenece a un usuario.

Una WatchList puede contener varias películas.

Una película puede aparecer en varias WatchLists.

Evitar insertar dos veces la misma película dentro de la misma WatchList.


# 16. Reproducción

El progreso debe poder persistirse por usuario y película.

Guardar como mínimo:

- IdUsuario.
- IdPelicula.
- SegundoActual.
- FechaUltimaReproduccion.

Debe existir como máximo un progreso actual por usuario y película.

El reproductor y mini-reproductor son principalmente responsabilidades
de interfaz.

La reproducción persistente entre navegaciones debe diseñarse evitando
reiniciar innecesariamente el elemento de video.


# 17. Código y estilo

Mantener nullable habilitado.

Manejar referencias nulas explícitamente.

Utilizar PascalCase para:

- clases;
- métodos;
- propiedades.

Utilizar camelCase para variables locales y parámetros.

Preferir nombres en español porque la estructura actual del proyecto se
encuentra en español.

No mezclar nombres duplicados en inglés y español para la misma
responsabilidad.

Ejemplo incorrecto:

MovieService
PeliculaServicio

Debe existir solamente PeliculaServicio.


# 18. Async / Await

Preferir métodos async para:

- acceso a base de datos;
- operaciones de entrada/salida;
- carga de información.

No crear métodos async que no ejecuten ninguna operación asíncrona.

Evitar:

Task.FromResult()

para simular operaciones asíncronas cuando ya existe acceso real a datos.


# 19. SOLID

Mantener responsabilidades separadas.

Controller:
coordina solicitudes y respuestas.

Service:
reglas de negocio.

Repository:
acceso a datos.

DbContext:
persistencia mediante Entity Framework Core.

DTO:
transferencia de información.

ViewModel:
información específica necesaria por una View.

Aplicar inyección de dependencias para depender de interfaces en lugar
de implementaciones concretas cuando corresponda.


# 20. Patrones utilizados

Mantener y documentar los patrones utilizados:

Repository Pattern:
en DAL para abstraer el acceso a datos.

Service Layer:
en BLL para concentrar reglas de negocio.

Dependency Injection:
para desacoplar servicios y repositorios.

DTO:
para transferir información entre capas.

MVC:
para separar interfaz, control y presentación.

Estos patrones deberán documentarse posteriormente en README.md.


# 21. Diseño visual

El diseño principal debe ser oscuro e inspirado en plataformas modernas
de streaming.

Mantener:

- navegación clara;
- tarjetas de películas;
- posters;
- responsive design;
- reproductor prominente;
- mini-reproductor persistente.

No copiar código o diseños completos de plataformas existentes.

Tomarlas únicamente como referencia visual.


# 22. Control de versiones

Utilizar un .gitignore apropiado.

No subir:

bin/
obj/
.vs/
bases de datos locales cuando no correspondan;
secretos;
credenciales.

Realizar commits pequeños con mensajes claros.

No modificar módulos no relacionados con la tarea solicitada.


# 23. Cambios realizados por asistentes de código

Antes de realizar cambios:

1. Leer este archivo instructions.md.
2. Revisar el módulo afectado.
3. Modificar únicamente los archivos necesarios.
4. No reestructurar otros módulos sin autorización.
5. No crear funcionalidades fuera del enunciado.
6. No duplicar entidades, DTOs, servicios o repositorios existentes.
7. Mantener la arquitectura MVC -> BLL -> DAL.
8. Compilar la solución después de cada etapa.
9. Informar exactamente:
   - archivos creados;
   - archivos modificados;
   - motivo de cada cambio;
   - resultado de compilación.
10. No generar ni aplicar migraciones sin autorización cuando el cambio
    altere la base de datos.
11. Toda nueva entidad creada dentro de DAL/Entidades debe declararse como public y respetar las reglas de navegación y accesibilidad definidas en este documento.


# 24. Regla principal de desarrollo

Trabajar módulo por módulo  

No implementar varias funcionalidades grandes simultáneamente.

Orden recomendado:

1. Base de datos y entidades.
2. Login.
3. Catálogo.
4. Detalle de película.
5. Perfiles de personas.
6. WatchLists.
7. Calificaciones y reseñas.
8. Reproducción.
9. Mini-reproductor.
10. Interacciones visuales.
11. Responsive.
12. Pruebas finales.
13. README.

Cada módulo debe compilar y probarse antes de continuar con el siguiente.


# 25. Restricciones e integridad de base de datos

Las reglas estructurales importantes deben existir también a nivel de
base de datos y no depender únicamente de validaciones de la interfaz.

Configurar mediante Fluent API cuando corresponda:

- campos obligatorios;
- longitudes máximas;
- índices únicos;
- relaciones;
- claves compuestas;
- comportamiento de eliminación.

Ejemplos:

Usuario:
- NombreUsuario obligatorio y único.
- Correo obligatorio y único.
- PasswordHash obligatorio.

Genero:
- Nombre obligatorio y único.

WatchList:
- Nombre obligatorio.
- combinación IdUsuario + Nombre única.

Calificacion:
- combinación IdUsuario + IdPelicula única.

Reproduccion:
- combinación IdUsuario + IdPelicula única.

PeliculaGenero:
- clave compuesta IdPelicula + IdGenero.

PeliculaActor:
- clave compuesta IdPelicula + IdActor.

WatchListPelicula:
- clave compuesta IdWatchList + IdPelicula.


# 26. Accesibilidad de entidades y propiedades de navegación

Reglas obligatorias:

- Todas las entidades persistentes ubicadas en
  G3_Proyecto_CineStreamCR.DAL/Entidades
  deben declararse como public.

- No utilizar internal class ni clases sin modificador de acceso para
  entidades administradas por Entity Framework Core.

- Las propiedades de navegación públicas no deben exponer tipos con
  menor nivel de accesibilidad.

- Las colecciones de navegación deben mantenerse públicas e inicializadas,
  por ejemplo:

  public ICollection<PeliculaActor> PeliculaActores { get; set; }
      = new List<PeliculaActor>();

- No solucionar errores de accesibilidad reduciendo arbitrariamente
  la visibilidad de propiedades de navegación.

- Si aparece un error CS0053 "Incoherencia de accesibilidad", revisar primero
  que la entidad utilizada por la propiedad pública también esté declarada
  como public.

- Esta regla debe aplicarse tanto a las entidades actuales como a cualquier
  entidad nueva agregada posteriormente.

Entidades actuales que deben respetar esta regla:

Usuario
Persona
Genero
Pelicula
PeliculaGenero
PeliculaActor
WatchList
WatchListPelicula
Calificacion
Reproduccion

# 27. Consultas del catálogo

La búsqueda, filtros, ordenamiento y paginación deben realizarse
preferiblemente en la consulta de base de datos y no después de cargar
todas las películas en memoria.

Construir la consulta mediante IQueryable y ejecutar ToListAsync()
únicamente después de aplicar:

- búsqueda por título;
- género;
- año;
- ordenamiento;
- Skip();
- Take().

Evitar traer toda la tabla con ToListAsync() para posteriormente filtrar
con LINQ en memoria.


# 28. Sesión y autenticación

La autenticación debe centralizarse en el módulo de usuarios.

Después de un login correcto, almacenar en sesión únicamente los datos
mínimos necesarios para identificar al usuario.

No almacenar contraseñas ni PasswordHash en Session, cookies, ViewBag,
ViewData ni TempData.

Las operaciones asociadas al usuario, como WatchLists, calificaciones y
progreso de reproducción, deben obtener el IdUsuario desde la sesión
autenticada y no confiar en un IdUsuario enviado manualmente desde una View.