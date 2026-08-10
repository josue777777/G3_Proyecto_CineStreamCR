# G3_Proyecto_CineStreamCR

# CineStream CR - Movie streaming platform inspired by HBO MAX. Built with ASP.NET Core MVC, Entity Framework Core and SQL Server.
Made by:

Grupo #3

* Fernando Carrillo Castro   
fcarrillo80174@ufide.ac.cr

* Andy González Jiménez   
agonzalez80996@ufide.ac.cr

* Josué Navarro Barrantes  
hnavarro60373@ufide.ac.cr

* Raul Castro Acuña   
rcastro70272@ufide.ac.cr        

# Repositorio

https://github.com/josue777777/G3_Proyecto_CineStreamCR

## Especificación básica del proyecto

CineStream CR es una aplicación web desarrollada con ASP.NET Core MVC y .NET 8. Su objetivo es simular las funciones principales de una plataforma de streaming, permitiendo el registro e inicio de sesión de usuarios, la consulta de películas, la visualización de perfiles de actores y directores, la reproducción de contenido, el registro de calificaciones y la administración de listas personalizadas.

### 1. Arquitectura del proyecto

El sistema utiliza una arquitectura por capas para separar la presentación, la lógica de negocio y el acceso a los datos.

La solución está formada por tres proyectos principales:

#### G3_Proyecto_CineStreamCR

Es una aplicación ASP.NET Core MVC y representa la capa de presentación.

Contiene los siguientes elementos:

* Controladores.
* Vistas Razor.
* Modelos utilizados por las vistas.
* Archivos CSS y JavaScript.
* Recursos visuales.
* Configuración general de la aplicación.
* Configuración de la inyección de dependencias.
* Configuración de sesiones y rutas.

Los controladores reciben las solicitudes realizadas por el usuario y utilizan los servicios de la capa de negocio para ejecutar las operaciones.

#### G3_Proyecto_CineStreamCR.BLL

Es una biblioteca de clases y representa la capa de lógica de negocio.

Contiene los siguientes elementos:

* Interfaces de servicios.
* Implementaciones de servicios.
* Validaciones.
* DTO.
* Reglas de negocio.
* Conversión de entidades a objetos utilizados por la presentación.

Entre los servicios principales se encuentran `UsuarioServicio`, `PeliculaService`, `PersonaService`, `WatchListService`, `CalificacionService` y `ReproduccionService`.

#### G3_Proyecto_CineStreamCR.DAL

Es una biblioteca de clases y representa la capa de acceso a datos.

Contiene los siguientes elementos:

* Entidades de la base de datos.
* Interfaces de repositorios.
* Implementaciones de repositorios.
* `ApplicationDbContext`.
* Configuración de Entity Framework Core.
* Migraciones.
* Relaciones y restricciones de las tablas.

Los repositorios se encargan de realizar las consultas y guardar los cambios en la base de datos.

#### Flujo de una operación

El flujo general de una operación es el siguiente:

**Vista → Controlador → Servicio BLL → Repositorio DAL → Base de datos**

Por ejemplo, cuando un usuario crea una lista de películas, la vista envía la información a `WatchListsController`. El controlador utiliza `IWatchListService` para ejecutar la operación. `WatchListService` aplica las validaciones y utiliza `IWatchListRepositorio` para guardar la información mediante `ApplicationDbContext`.

Los tres proyectos principales utilizan .NET 8.

### 2. Librerías y paquetes NuGet utilizados

El proyecto utiliza los siguientes paquetes NuGet:

#### Microsoft.EntityFrameworkCore 8.0.11

Permite trabajar con la base de datos mediante entidades, consultas LINQ y seguimiento de cambios.

#### Microsoft.EntityFrameworkCore.Sqlite 8.0.11

Proporciona la integración entre Entity Framework Core y la base de datos SQLite.

#### Microsoft.EntityFrameworkCore.Design 8.0.11

Incluye las herramientas de diseño necesarias para crear y administrar migraciones de Entity Framework Core.

#### Microsoft.EntityFrameworkCore.Tools 8.0.11

Permite ejecutar comandos de Entity Framework Core desde las herramientas de desarrollo.

#### SQLitePCLRaw.lib.e_sqlite3 3.53.3

Incluye los componentes nativos necesarios para ejecutar SQLite dentro del proyecto.

#### Microsoft.Extensions.Identity.Core 8.0.11

Proporciona la clase `PasswordHasher`, utilizada para generar y verificar el hash de las contraseñas.

#### AutoMapper 16.1.1

Está instalado para apoyar el mapeo entre entidades y DTO. Actualmente, los mapeos principales se realizan manualmente dentro de los servicios.

También se utilizan las siguientes librerías del lado del cliente:

* Bootstrap 5.3.3 para componentes visuales y diseño adaptable.
* jQuery 3.7.1 para funciones de JavaScript.
* jQuery Validation 1.21.0 para la validación de formularios.
* jQuery Validation Unobtrusive para integrar las validaciones de ASP.NET Core con jQuery.
* Google Fonts para utilizar la fuente Outfit en diferentes pantallas.

### 3. Principios SOLID aplicados

#### Principio de responsabilidad única

Cada clase tiene una responsabilidad definida dentro del sistema.

Los controladores reciben las solicitudes y seleccionan las vistas. Los servicios contienen las reglas de negocio y validaciones. Los repositorios realizan las consultas a la base de datos. Los DTO transportan únicamente la información necesaria entre las capas.

Por ejemplo, `WatchListsController` no guarda directamente la información en la base de datos. El controlador envía la operación a `WatchListService`, que realiza las validaciones y utiliza `WatchListRepositorio` para guardar los cambios.

Esta separación facilita la lectura, mantenimiento y modificación del código.

#### Principio abierto/cerrado

El sistema utiliza interfaces que permiten cambiar o ampliar las implementaciones sin modificar completamente las clases que las utilizan.

Los controladores trabajan con interfaces como `IWatchListService`, `IPeliculaService`, `IPersonaService` e `IUsuarioServicio`.

Esto permite crear una implementación diferente de un servicio o repositorio manteniendo el mismo contrato.

Este principio se aplica de forma básica, ya que algunas funcionalidades nuevas todavía pueden requerir agregar métodos a las interfaces existentes.

#### Principio de sustitución de Liskov

Las clases concretas cumplen los contratos definidos por sus interfaces.

Algunos ejemplos son:

* `IWatchListService` es implementada por `WatchListService`.
* `IPeliculaService` es implementada por `PeliculaService`.
* `IPersonaService` es implementada por `PersonaService`.
* `IUsuarioServicio` es implementada por `UsuarioServicio`.

Una implementación puede ser sustituida por otra siempre que respete los mismos métodos y tipos de retorno.

El proyecto no utiliza jerarquías complejas de herencia, por lo que este principio se presenta principalmente mediante interfaces.

#### Principio de segregación de interfaces

El sistema utiliza interfaces separadas para cada módulo y no una única interfaz general con todas las operaciones.

Entre las interfaces utilizadas se encuentran:

* `IUsuarioServicio`.
* `IPeliculaService`.
* `IPersonaService`.
* `IWatchListService`.
* `ICalificacionService`.
* `IReproduccionService`.
* `IUsuarioRepositorio`.
* `IPeliculaRepositorio`.
* `IPersonaRepositorio`.
* `IWatchListRepositorio`.

Esta separación evita que una clase dependa de métodos que no necesita.

#### Principio de inversión de dependencias

Los controladores y servicios dependen principalmente de interfaces y no directamente de implementaciones concretas.

Por ejemplo, `WatchListsController` recibe una dependencia de tipo `IWatchListService`.

La relación entre la interfaz y su implementación se configura en `Program.cs` mediante `AddScoped`.

La configuración utilizada es la siguiente:

* `IUsuarioRepositorio` con `UsuarioRepositorio`.
* `IUsuarioServicio` con `UsuarioServicio`.
* `IPeliculaRepositorio` con `PeliculaRepositorio`.
* `IPeliculaService` con `PeliculaService`.
* `IWatchListRepositorio` con `WatchListRepositorio`.
* `IWatchListService` con `WatchListService`.
* `IPersonaRepositorio` con `PersonaRepositorio`.
* `IPersonaService` con `PersonaService`.
* `ICalificacionService` con `CalificacionService`.
* `IReproduccionService` con `ReproduccionService`.

Los servicios de calificaciones y reproducción reciben directamente `ApplicationDbContext`, pero este también es proporcionado mediante inyección de dependencias.

### 4. Patrones de diseño utilizados

#### Patrón MVC

El patrón Modelo-Vista-Controlador es la base de la aplicación.

Los modelos y DTO representan los datos, las vistas Razor muestran la información al usuario y los controladores reciben las solicitudes y coordinan las operaciones.

Esta separación evita mezclar el código de la interfaz con la lógica de negocio.

#### Patrón Repository

El patrón Repository se utiliza para separar las consultas a la base de datos de la lógica de negocio.

Los repositorios principales son:

* `UsuarioRepositorio`.
* `PeliculaRepositorio`.
* `PersonaRepositorio`.
* `WatchListRepositorio`.

Por ejemplo, `PeliculaRepositorio` contiene las consultas necesarias para buscar películas, aplicar filtros, ordenar resultados y realizar la paginación del catálogo.

#### Patrón Service Layer

La lógica de negocio se organiza mediante servicios.

Los servicios principales son:

* `UsuarioServicio`.
* `PeliculaService`.
* `PersonaService`.
* `WatchListService`.
* `CalificacionService`.
* `ReproduccionService`.

Estos servicios validan la información y coordinan las operaciones antes de guardar o consultar datos.

Por ejemplo, `WatchListService` verifica que un usuario no tenga dos listas con el mismo nombre, valida que las películas existan y comprueba que las listas seleccionadas pertenezcan al usuario autenticado.

#### Inyección de dependencias

La inyección de dependencias permite que ASP.NET Core cree y proporcione automáticamente los servicios, repositorios y el contexto que necesita cada clase.

Las dependencias se registran en `Program.cs` mediante `AddScoped`.

Esto reduce el acoplamiento y facilita cambiar una implementación en el futuro.

#### Patrón DTO

Los objetos de transferencia de datos permiten enviar únicamente la información necesaria entre las capas.

Algunos DTO utilizados son:

* `LoginDto`.
* `RegistroDto`.
* `PeliculaCatalogoDto`.
* `CatalogoFiltroDto`.
* `PersonaResumenDto`.
* `PersonaDetalleDto`.
* `WatchListResumenDto`.
* `WatchListDetalleDto`.
* `WatchListFormDto`.

El uso de DTO evita enviar todas las propiedades de las entidades directamente a las vistas o formularios.

#### Unit of Work mediante DbContext

El proyecto no contiene una clase `UnitOfWork` personalizada. Sin embargo, `ApplicationDbContext` cumple esta función mediante Entity Framework Core.

El contexto mantiene el seguimiento de los cambios realizados en las entidades y los confirma mediante `SaveChangesAsync`.

Esto permite guardar varias modificaciones como parte de una misma operación.

#### Patrón Post/Redirect/Get

Después de ejecutar una operación POST, como crear, editar o eliminar una lista, el controlador redirige al usuario hacia una acción GET.

Este comportamiento evita que una operación se vuelva a ejecutar si el usuario actualiza la página después de enviar el formulario.

### 5. Decisiones de diseño de base de datos

#### Uso de SQLite

Se seleccionó SQLite porque permite trabajar con una base de datos local almacenada en un único archivo llamado `CineStreamCR.db`.

Esta decisión facilita la ejecución y entrega del proyecto, ya que no es necesario configurar un servidor externo de base de datos.

La cadena de conexión se encuentra definida en `appsettings.json`.

#### Uso de Entity Framework Core Code First

La base de datos se genera a partir de las entidades definidas en el proyecto DAL.

Las relaciones, restricciones y longitudes máximas se configuran mediante Fluent API dentro de `ApplicationDbContext`.

El proyecto contiene una migración inicial para crear la estructura de la base de datos. También utiliza `DbInitializer` para insertar personas, géneros, películas y un usuario de demostración cuando las tablas están vacías.

#### Organización de las tablas

La base de datos contiene las siguientes tablas principales:

* `Usuarios`.
* `Personas`.
* `Generos`.
* `Peliculas`.
* `PeliculaGeneros`.
* `PeliculaActores`.
* `WatchLists`.
* `WatchListPeliculas`.
* `Calificaciones`.
* `Reproducciones`.

Esta organización evita almacenar información repetida y permite mantener separadas las responsabilidades de cada entidad.

#### Relaciones entre películas, géneros y personas

Una película puede pertenecer a varios géneros y un género puede estar relacionado con varias películas.

Esta relación se representa mediante la tabla intermedia `PeliculaGenero`.

Una película también puede tener varios actores y un actor puede participar en varias películas.

Esta relación se representa mediante `PeliculaActor`, que también guarda el nombre del personaje interpretado.

La entidad `Persona` se utiliza para actores y directores. Una persona puede dirigir varias películas y también puede participar como actor. Esto evita duplicar la información personal en tablas diferentes.

#### Relaciones de las listas personalizadas

Cada usuario puede crear varias WatchLists, pero cada lista pertenece únicamente a un usuario.

Una lista puede contener varias películas y una película puede estar incluida en varias listas.

Esta relación se representa mediante la tabla intermedia `WatchListPelicula`.

La llave primaria compuesta por `IdWatchList` e `IdPelicula` evita que una película se registre dos veces dentro de la misma lista.

#### Llaves e índices únicos

Todas las entidades principales utilizan una llave primaria numérica.

También se definieron índices únicos para evitar datos duplicados:

* El nombre de usuario debe ser único.
* El correo electrónico debe ser único.
* El nombre del género debe ser único.
* Un usuario no puede tener dos WatchLists con el mismo nombre.
* Un usuario solamente puede mantener una calificación por película.
* Un usuario solamente puede mantener un progreso de reproducción por película.

#### Eliminación en cascada y eliminación restringida

Se utiliza eliminación en cascada cuando un registro dependiente no debe existir sin su registro principal.

Por ejemplo, cuando se elimina una WatchList, también se eliminan sus relaciones almacenadas en `WatchListPelicula`.

En las relaciones entre personas y películas se utiliza eliminación restringida. Esto evita eliminar accidentalmente un actor o director que todavía está relacionado con una película.

#### Seguridad de las contraseñas

Las contraseñas no se almacenan en texto plano.

La entidad `Usuario` contiene la propiedad `PasswordHash`, donde se guarda el resultado generado por `PasswordHasher<Usuario>`.

Durante el inicio de sesión, la contraseña ingresada se compara con el hash almacenado mediante `VerifyHashedPassword`.

#### Almacenamiento de calificaciones

Cada calificación individual se guarda en la tabla `Calificaciones`.

Cuando un usuario agrega o actualiza una reseña, el sistema vuelve a calcular el promedio de la película.

El promedio se almacena en la propiedad `Rating` de la entidad `Pelicula`. Esto permite mostrar la calificación en el catálogo sin calcular nuevamente el promedio en cada consulta.

#### Almacenamiento del progreso de reproducción

La tabla `Reproducciones` guarda el segundo en el que cada usuario detuvo una película y la fecha de la última reproducción.

El índice único formado por `IdUsuario` e `IdPelicula` garantiza que solo exista un progreso por cada combinación de usuario y película.

#### Restricciones de campos

Mediante Fluent API se definieron campos obligatorios, campos opcionales y longitudes máximas.

Entre las restricciones principales se encuentran:

* Nombre de usuario con un máximo de 50 caracteres.
* Correo electrónico con un máximo de 150 caracteres.
* Título de película con un máximo de 200 caracteres.
* Sinopsis con un máximo de 3000 caracteres.
* Nombre de WatchList con un máximo de 100 caracteres.
* Descripción de WatchList con un máximo de 500 caracteres.
* Reseña con un máximo de 2000 caracteres.

Estas restricciones ayudan a mantener la integridad de la información almacenada en la base de datos.
