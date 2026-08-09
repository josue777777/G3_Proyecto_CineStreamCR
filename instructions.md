# CineStream CR - Instrucciones y buenas prácticas

Este documento define las reglas permanentes de desarrollo para CineStream CR.

Su objetivo es mantener una arquitectura clara, mantenible, escalable y
coherente con los contenidos desarrollados en el curso y con el enunciado
oficial del proyecto.

Antes de realizar cambios importantes en el proyecto, revisar estas reglas.

Este documento debe contener principios y decisiones permanentes.

No debe utilizarse como:

- bitácora de errores;
- historial de commits;
- registro de problemas puntuales;
- lista de todos los archivos existentes;
- documentación temporal de una implementación específica.


# 1. Alcance general

CineStream CR es una plataforma web de streaming de películas.

La aplicación debe inspirarse visualmente en plataformas modernas de streaming,
sin copiar directamente código, diseños completos o recursos protegidos de
terceros.

Las funcionalidades deben mantenerse dentro del alcance definido por el
enunciado oficial.

Antes de incorporar una funcionalidad nueva, comprobar:

1. si aparece directamente en el enunciado;
2. si es técnicamente necesaria para cumplir un requerimiento existente;
3. si respeta la arquitectura del proyecto.

Si no cumple ninguna de estas condiciones, no implementarla sin autorización.


# 2. Arquitectura

Mantener una arquitectura por capas con responsabilidades claramente
separadas.

Arquitectura conceptual:

Presentación
    ↓
Lógica de negocio
    ↓
Acceso a datos
    ↓
Entity Framework Core
    ↓
Base de datos

La capa de presentación se basa principalmente en ASP.NET Core MVC.

La capa de lógica de negocio contiene las reglas y operaciones propias del
dominio.

La capa de acceso a datos administra la persistencia mediante repositorios y
Entity Framework Core.

Reglas:

- Los Controllers no deben acceder directamente al DbContext.
- Las Views no deben acceder a repositorios.
- Las Views no deben contener lógica de negocio.
- Los Controllers coordinan solicitudes, respuestas y navegación.
- Los Services contienen reglas de negocio.
- Los Repositories administran acceso a datos.
- El DbContext administra la interacción de Entity Framework Core con la base
  de datos.

Evitar dependencias que rompan el flujo natural entre capas.


# 3. API

Una Web API independiente no es obligatoria para la arquitectura base.

No agregar una API únicamente por complejidad técnica aparente o por preferencia
personal.

Agregar una API solamente cuando:

- exista un requerimiento que realmente la necesite;
- facilite una funcionalidad dinámica importante;
- exista una razón arquitectónica clara;
- haya sido previamente autorizada.

Si se incorpora una API:

- debe reutilizar la lógica de negocio existente;
- no debe duplicar Services;
- no debe duplicar Repositories;
- no debe duplicar reglas de negocio;
- debe mantener la separación por capas.


# 4. Organización del código

Organizar el código por responsabilidad.

Ejemplo conceptual:

Presentación
    Controllers/
    Models/
    ViewModels/
    Views/
    recursos frontend/

Negocio
    DTOs/
    Services/
    Interfaces/
    configuración de mapeo/

Datos
    Entidades/
    Repositories/
    Interfaces/
    Data/
    Migrations/

La estructura puede crecer conforme aparezcan nuevos módulos.

No actualizar este documento únicamente para registrar cada archivo nuevo.

Evitar carpetas innecesarias cuando todavía no exista suficiente código que
justifique la separación.


# 5. Entity Framework Core

Utilizar Entity Framework Core para la persistencia.

Registrar el DbContext mediante inyección de dependencias.

No hardcodear cadenas de conexión dentro del DbContext.

Obtener la configuración desde mecanismos como:

- archivos de configuración;
- configuración específica de desarrollo;
- User Secrets;
- variables de entorno.

Las relaciones, restricciones, índices y claves compuestas deben configurarse
principalmente mediante Fluent API cuando corresponda.

Utilizar migraciones para crear y evolucionar el esquema de base de datos.

No modificar manualmente migraciones que ya hayan sido aplicadas y compartidas
como parte estable del proyecto.

Entity Framework Core y sus migraciones representan la fuente real del esquema.

Los scripts SQL auxiliares deben considerarse documentación o apoyo, salvo que
se acuerde explícitamente otra estrategia.


# 6. Base de datos

Utilizar SQLite como repositorio principal mientras siga siendo la tecnología
seleccionada para el proyecto.

Diseñar la base de datos buscando:

- integridad;
- normalización razonable;
- claridad;
- relaciones explícitas;
- ausencia de duplicación innecesaria;
- facilidad de mantenimiento.

No duplicar entidades que representen el mismo concepto cuando una relación
pueda resolver correctamente la diferencia de comportamiento.

Las reglas importantes de integridad no deben depender solamente de la
interfaz de usuario.


# 7. Entidades y relaciones

Todas las entidades persistentes administradas por Entity Framework Core deben
ser públicas.

Las propiedades públicas no deben exponer tipos con menor accesibilidad.

Las colecciones de navegación deben mantenerse inicializadas cuando corresponda.

Ejemplo conceptual:

public ICollection<TEntidadRelacionada> Elementos { get; set; }
    = new List<TEntidadRelacionada>();

Las propiedades de navegación deben representar relaciones reales del dominio.

Evitar relaciones redundantes o propiedades de navegación que no sean
necesarias.


# 8. Integridad y restricciones

Configurar a nivel de base de datos cuando corresponda:

- claves primarias;
- claves foráneas;
- campos obligatorios;
- longitudes máximas;
- índices;
- índices únicos;
- claves compuestas;
- relaciones muchos a muchos;
- comportamiento de eliminación;
- restricciones de duplicidad.

Las validaciones de interfaz no sustituyen las restricciones estructurales de
base de datos.


# 9. Consultas

Utilizar IQueryable cuando permita construir consultas dinámicas.

Esto es especialmente importante para:

- búsquedas;
- filtros;
- ordenamiento;
- paginación.

Aplicar filtros y ordenamiento antes de ejecutar la consulta contra la base de
datos.

Evitar obtener una colección completa para posteriormente filtrar en memoria
cuando la operación pueda realizarse desde la base de datos.

Preferir métodos asíncronos de Entity Framework Core para operaciones de
entrada y salida.


# 10. Tracking

Utilizar AsNoTracking() en consultas exclusivamente de lectura.

Ejemplos:

- catálogos;
- búsquedas;
- detalles de solo lectura;
- perfiles;
- listados.

Mantener tracking cuando una entidad obtenida vaya a:

- modificarse;
- eliminarse;
- actualizarse dentro de la misma unidad de trabajo.

Entity Framework Core realiza tracking por defecto.

Utilizar AsTracking() explícitamente solamente cuando exista una razón clara.


# 11. Repositories

Los Repositories son responsables del acceso a datos.

Responsabilidades válidas:

- consultar información;
- buscar por identificadores;
- construir consultas;
- agregar registros;
- actualizar registros;
- eliminar registros;
- guardar cambios.

No colocar reglas de negocio dentro de Repositories.

No utilizar datos simulados o listas estáticas una vez exista persistencia real
para ese módulo.


# 12. Services

Los Services representan la capa de lógica de negocio.

Deben encargarse de:

- validaciones de negocio;
- decisiones de dominio;
- prevención de operaciones inválidas;
- coordinación entre repositorios;
- transformación de datos cuando corresponda.

Los Controllers deben delegar las reglas de negocio a los Services.

Evitar Controllers con lógica compleja.


# 13. DTOs

Los DTOs representan datos transferidos entre capas.

No deben utilizarse como sustituto automático de todas las clases existentes.

Crear DTOs cuando:

- sea necesario controlar qué información se expone;
- una operación requiera datos diferentes;
- se necesite desacoplar persistencia y presentación.

Evitar crear múltiples DTOs que contengan exactamente la misma información sin
una necesidad real.

Organizar los DTOs por responsabilidad o módulo cuando el tamaño del proyecto lo
justifique.


# 14. ViewModels

Los ViewModels representan información específica necesaria para una View.

Pueden combinar información proveniente de diferentes DTOs cuando la interfaz lo
requiera.

No colocar reglas de negocio dentro de ViewModels.

No utilizar entidades persistentes directamente en la interfaz cuando un
ViewModel permita mantener una separación adecuada.


# 15. Mapeo

Centralizar la configuración de mapeo entre entidades, DTOs y otros modelos
cuando se utilice una herramienta de mapeo.

Evitar mapeos manuales repetitivos si pueden resolverse de manera clara mediante
la configuración existente.

Utilizar configuraciones específicas únicamente cuando las propiedades no
coincidan directamente o sea necesario transformar información.


# 16. Validaciones

Aplicar validaciones tanto en cliente como en servidor cuando corresponda.

Utilizar DataAnnotations para validaciones básicas como:

- campos requeridos;
- longitudes;
- rangos;
- formatos de correo;
- formatos válidos.

Nunca confiar únicamente en JavaScript.

Las reglas importantes del dominio deben validarse también en la capa de lógica
de negocio.

Las restricciones estructurales importantes deben reforzarse en base de datos
cuando corresponda.


# 17. Seguridad

Nunca almacenar contraseñas en texto plano.

Guardar únicamente hashes seguros de contraseña.

La información relacionada con credenciales nunca debe exponerse innecesariamente
a la capa de presentación.

No almacenar contraseñas ni hashes dentro de:

- ViewBag;
- ViewData;
- TempData;
- Session;
- modelos enviados al navegador cuando no sean necesarios.

La identidad del usuario debe determinarse mediante el mecanismo de
autenticación o sesión utilizado por la aplicación.

Las operaciones asociadas a un usuario no deben confiar ciegamente en un
identificador enviado desde la interfaz.


# 18. Sesión y autenticación

Centralizar la autenticación dentro del módulo correspondiente.

Después de una autenticación correcta, almacenar en sesión solamente la
información mínima necesaria para identificar al usuario.

Las operaciones personalizadas deben utilizar la identidad autenticada.

Evitar duplicar lógica de autenticación entre Controllers.


# 19. Async / Await

Preferir async/await para:

- acceso a base de datos;
- operaciones de entrada y salida;
- llamadas HTTP;
- lectura o escritura de archivos;
- operaciones externas.

No crear métodos async si no contienen operaciones asíncronas reales.

No utilizar Task.FromResult() únicamente para aparentar comportamiento
asíncrono cuando exista una fuente de datos real.


# 20. Código y estilo

Mantener nullable habilitado.

Manejar referencias nulas explícitamente.

Utilizar PascalCase para:

- clases;
- métodos;
- propiedades.

Utilizar camelCase para:

- variables locales;
- parámetros.

Mantener una convención de nombres consistente en todo el proyecto.

No mantener dos clases o servicios diferentes para representar exactamente la
misma responsabilidad.

Preferir claridad antes que abreviaciones innecesarias.


# 21. SOLID

Aplicar SOLID de forma práctica.

No introducir abstracciones únicamente para afirmar que se utiliza SOLID.

Buscar especialmente:

Single Responsibility:
cada clase debe tener una responsabilidad principal clara.

Open/Closed:
evitar modificar componentes no relacionados cuando se extiende una
funcionalidad.

Liskov Substitution:
las implementaciones deben respetar los contratos definidos por sus
abstracciones.

Interface Segregation:
evitar interfaces excesivamente grandes.

Dependency Inversion:
preferir dependencias mediante abstracciones cuando aporten desacoplamiento real.


# 22. Patrones

Utilizar y documentar solamente patrones realmente presentes en el proyecto.

Patrones esperados de forma natural:

Repository Pattern:
abstracción del acceso a datos.

Service Layer:
centralización de reglas de negocio.

Dependency Injection:
resolución y desacoplamiento de dependencias.

DTO:
transferencia controlada de información.

MVC:
separación entre presentación, control y modelo de interfaz.

No agregar patrones únicamente para aumentar la complejidad del proyecto.


# 23. Paquetes y dependencias

Mantener las dependencias compatibles con el framework utilizado por la
solución.

Los paquetes pertenecientes a una misma familia tecnológica deben mantener
versiones compatibles entre sí.

Evitar mezclar versiones mayores incompatibles.

No actualizar dependencias automáticamente solo porque exista una versión más
reciente.

Antes de actualizar:

1. revisar compatibilidad;
2. realizar el cambio;
3. restaurar dependencias;
4. recompilar;
5. probar el módulo afectado.

Revisar advertencias de vulnerabilidad antes de la entrega final.


# 24. Diseño visual

El diseño principal debe utilizar un tema oscuro inspirado en plataformas
modernas de streaming.

Mantener:

- navegación clara;
- jerarquía visual;
- tarjetas de películas;
- pósteres;
- diseño responsive;
- reproductor prominente;
- mini-reproductor persistente.

Las plataformas comerciales sirven únicamente como referencia visual y de
experiencia.

No copiar código, interfaces completas ni recursos protegidos de terceros.


# 25. Control de versiones

Mantener un .gitignore apropiado.

No versionar:

- binarios generados;
- carpetas temporales;
- archivos del entorno de desarrollo;
- secretos;
- credenciales;
- configuraciones privadas;
- archivos locales que no correspondan al proyecto.

Las bases de datos locales deben versionarse únicamente si existe una decisión
explícita del equipo para hacerlo.

Realizar commits pequeños y con mensajes descriptivos.

No mezclar cambios no relacionados dentro del mismo commit cuando pueda
evitarse.


# 26. Reglas para asistentes de código

Antes de realizar cambios:

1. Leer completamente este documento.
2. Revisar el módulo y arquitectura afectados.
3. Modificar únicamente lo necesario para la tarea actual.
4. No reestructurar otros módulos sin autorización.
5. No duplicar responsabilidades existentes.
6. Mantener la separación por capas.
7. Mantener las convenciones existentes.
8. Utilizar código asíncrono cuando corresponda.
9. Respetar las reglas de seguridad.
10. No generar ni aplicar migraciones sin autorización cuando cambien el
    esquema.
11. Compilar después de cambios importantes.
12. Informar qué fue creado o modificado y por qué.

Antes de crear una funcionalidad que no exista actualmente:

1. Compararla con los requerimientos principales definidos al final de este
   documento.
2. Determinar si forma parte del enunciado.
3. Si no forma parte del enunciado ni es técnicamente necesaria para cumplirlo,
   detenerse e informar que la propuesta se encuentra fuera del alcance.
4. No implementarla sin autorización.

Este documento no debe utilizarse para almacenar errores puntuales, commits,
resultados de compilaciones o soluciones temporales.


# 27. Método de desarrollo

Trabajar módulo por módulo.

No implementar simultáneamente múltiples funcionalidades grandes cuando puedan
desarrollarse y probarse por separado.

Orden recomendado:

1. modelo de datos;
2. autenticación;
3. catálogo;
4. detalle de contenido;
5. perfiles relacionados;
6. listas personalizadas;
7. calificaciones y reseñas;
8. reproducción;
9. reproductor persistente;
10. interacción visual;
11. responsive;
12. pruebas finales;
13. documentación.

Cada módulo debe compilar y probarse antes de continuar con el siguiente.


# 28. Documentación final

Antes de la entrega debe existir un README.

Debe documentar como mínimo:

- integrantes finales;
- repositorio público;
- arquitectura utilizada;
- tipos de proyectos presentes en la solución;
- dependencias y paquetes utilizados;
- principios SOLID realmente aplicados;
- patrones de diseño realmente utilizados;
- decisiones principales de base de datos;
- instrucciones básicas de ejecución.

No documentar tecnologías o patrones que no se encuentren realmente
implementados.


# -----------------------------------------------------------------------

# 29. Requerimientos principales del proyecto

Esta sección representa el alcance funcional esperado según el enunciado oficial.

Debe utilizarse como referencia antes de implementar nuevas funcionalidades.

Si una propuesta no está relacionada con alguno de estos requerimientos y no es
técnicamente necesaria para cumplirlos, debe considerarse fuera del alcance y
no debe implementarse sin autorización.


## 29.1 Login

El sistema debe permitir:

- inicio de sesión mediante email o username y contraseña;
- validación de campos en cliente;
- validación de campos en servidor;
- redirección al catálogo después de autenticación correcta;
- mensajes descriptivos cuando el usuario no exista;
- mensajes descriptivos cuando la contraseña sea incorrecta.


## 29.2 Catálogo de películas

Debe existir un catálogo principal.

Debe incluir:

- listado paginado;
- póster;
- título;
- año;
- duración;
- calificación.

Debe permitir:

- búsqueda en tiempo real por título;
- filtro por género;
- filtro por año de estreno;
- ordenamiento por título;
- ordenamiento por año;
- ordenamiento por calificación;
- orden ascendente;
- orden descendente;
- acceso al detalle desde la tarjeta de película;
- agregar películas a listas personalizadas;
- mostrar visualmente cuando una película ya pertenezca a alguna lista.


## 29.3 Detalle de película

Debe mostrar:

- póster;
- título;
- sinopsis;
- duración;
- año;
- géneros;
- rating;
- director;
- foto del director;
- elenco principal.

Debe permitir:

- acceder al perfil del director;
- acceder al perfil de los actores;
- agregar la película a una lista;
- quitarla de una lista;
- asignar una calificación de 1 a 10;
- escribir una reseña opcional;
- iniciar la reproducción mediante un botón visible.


## 29.4 Directores y actores

Cada película debe estar relacionada con:

- un director;
- elenco principal.

Los perfiles deben mostrar:

- foto;
- nombre;
- nacionalidad;
- biografía;
- fecha de nacimiento.

El perfil de director debe mostrar las películas dirigidas.

El perfil de actor debe mostrar:

- películas en las que participó;
- personaje interpretado.


## 29.5 WatchLists

El usuario debe poder:

- crear listas personalizadas;
- indicar nombre;
- indicar descripción;
- editar nombre;
- editar descripción;
- eliminar una lista con confirmación;
- agregar películas;
- quitar películas;
- agregar una película a varias listas;
- consultar las películas de cada lista.


## 29.6 Calificaciones y reseñas

El usuario debe poder:

- calificar películas entre 1 y 10;
- escribir una reseña opcional.

La aplicación debe evitar registros duplicados innecesarios para una misma
interacción de usuario y película.


## 29.7 Reproducción de películas

Debe existir un reproductor de video embebido.

Debe incluir:

- Play;
- Pause;
- avanzar;
- retroceder;
- barra de progreso interactiva;
- control de volumen;
- película anterior;
- película siguiente.

La reproducción debe mantenerse mientras el usuario navega dentro de la
aplicación.


## 29.8 Mini-reproductor persistente

Mientras exista reproducción activa debe mostrarse un mini-reproductor
persistente.

Debe incluir como mínimo:

- título de la película;
- póster o miniatura;
- controles básicos.

Debe permitir continuar la reproducción sin reiniciarla innecesariamente al
cambiar de sección.


## 29.9 Interacción con el usuario

La aplicación debe proporcionar una experiencia fluida.

Debe incluir:

- notificaciones visuales o toasts;
- mensajes ante errores de autenticación;
- notificación al agregar contenido a listas;
- notificación al calificar;
- indicador de carga durante operaciones asíncronas cuando corresponda.


## 29.10 Diseño y experiencia

La interfaz debe:

- utilizar un tema oscuro;
- inspirarse visualmente en plataformas de streaming existentes;
- funcionar correctamente en escritorio;
- adaptarse a dispositivos móviles;
- mantener una navegación clara;
- dar especial importancia visual al contenido audiovisual y al reproductor.


## 29.11 Regla de control de alcance

Antes de crear cualquier funcionalidad nueva, comparar la solicitud con esta
sección.

Clasificar la nueva funcionalidad como:

REQUERIDA:
aparece directamente en el enunciado.

DE APOYO:
no aparece literalmente, pero es técnicamente necesaria para implementar un
requerimiento.

FUERA DE ALCANCE:
no aparece en el enunciado y no es necesaria para cumplir ningún requerimiento.

Las funcionalidades clasificadas como FUERA DE ALCANCE no deben implementarse
sin autorización explícita.

Ejemplos de funcionalidades fuera del alcance salvo autorización:

- pagos;
- suscripciones;
- planes premium;
- recomendaciones mediante inteligencia artificial;
- chat;
- red social;
- perfiles infantiles;
- descarga offline;
- sistemas de anuncios;
- funcionalidades administrativas no solicitadas.

# -----------------------------------------------------------------------