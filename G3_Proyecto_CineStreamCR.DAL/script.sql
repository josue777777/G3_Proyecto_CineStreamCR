-- =========================================================
-- BASE DE DATOS: CineStream CR
-- Finalidad: almacenar la información necesaria para login,
-- catálogo de películas, detalle, perfiles, watchlists,
-- calificaciones, reseñas y reproducción.
-- =========================================================


-- USUARIOS
-- Parte del proyecto: Login y personalización de WatchLists.
-- Finalidad: guardar los datos básicos del usuario que inicia sesión.
CREATE TABLE Usuarios (
    IdUsuario INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreUsuario TEXT NOT NULL UNIQUE,
    Correo TEXT NOT NULL UNIQUE,
    Contrasenna TEXT NOT NULL
);


-- DIRECTORES
-- Parte del proyecto: Detalle de película y perfil del director.
-- Finalidad: guardar la información del director de cada película.
CREATE TABLE Directores (
    IdDirector INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Nacionalidad TEXT,
    Biografia TEXT,
    FechaNacimiento TEXT,
    FotoUrl TEXT
);


-- ACTORES
-- Parte del proyecto: Detalle de película y perfil del actor.
-- Finalidad: guardar la información de los actores principales.
CREATE TABLE Actores (
    IdActor INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Nacionalidad TEXT,
    Biografia TEXT,
    FechaNacimiento TEXT,
    FotoUrl TEXT
);


-- GENEROS
-- Parte del proyecto: Catálogo, filtros y clasificación.
-- Finalidad: permitir clasificar películas por género.
CREATE TABLE Generos (
    IdGenero INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL UNIQUE
);


-- PELICULAS
-- Parte del proyecto: Catálogo, detalle y reproducción.
-- Finalidad: guardar la información principal de cada película.
CREATE TABLE Peliculas (
    IdPelicula INTEGER PRIMARY KEY AUTOINCREMENT,
    Titulo TEXT NOT NULL,
    Sinopsis TEXT,
    Anio INTEGER NOT NULL,
    DuracionMinutos INTEGER NOT NULL,
    PosterUrl TEXT,
    VideoUrl TEXT,
    Rating REAL DEFAULT 0,
    IdDirector INTEGER NOT NULL,
    FOREIGN KEY (IdDirector) REFERENCES Directores(IdDirector)
);


-- PELICULA_GENEROS
-- Parte del proyecto: Filtro por género y detalle de película.
-- Finalidad: permitir que una película tenga uno o varios géneros.
CREATE TABLE PeliculaGeneros (
    IdPelicula INTEGER NOT NULL,
    IdGenero INTEGER NOT NULL,
    PRIMARY KEY (IdPelicula, IdGenero),
    FOREIGN KEY (IdPelicula) REFERENCES Peliculas(IdPelicula),
    FOREIGN KEY (IdGenero) REFERENCES Generos(IdGenero)
);


-- PELICULA_ACTORES
-- Parte del proyecto: Elenco principal y perfil del actor.
-- Finalidad: relacionar películas con actores y personaje interpretado.
CREATE TABLE PeliculaActores (
    IdPelicula INTEGER NOT NULL,
    IdActor INTEGER NOT NULL,
    Personaje TEXT,
    PRIMARY KEY (IdPelicula, IdActor),
    FOREIGN KEY (IdPelicula) REFERENCES Peliculas(IdPelicula),
    FOREIGN KEY (IdActor) REFERENCES Actores(IdActor)
);


-- WATCHLISTS
-- Parte del proyecto: Gestión de listas personalizadas.
-- Finalidad: permitir que cada usuario cree listas de películas.
CREATE TABLE WatchLists (
    IdWatchList INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Descripcion TEXT,
    IdUsuario INTEGER NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);


-- WATCHLIST_PELICULAS
-- Parte del proyecto: Agregar o quitar películas de una WatchList.
-- Finalidad: relacionar las listas del usuario con sus películas.
CREATE TABLE WatchListPeliculas (
    IdWatchList INTEGER NOT NULL,
    IdPelicula INTEGER NOT NULL,
    PRIMARY KEY (IdWatchList, IdPelicula),
    FOREIGN KEY (IdWatchList) REFERENCES WatchLists(IdWatchList),
    FOREIGN KEY (IdPelicula) REFERENCES Peliculas(IdPelicula)
);


-- CALIFICACIONES
-- Parte del proyecto: Calificación de películas.
-- Finalidad: guardar la puntuación que un usuario da a una película.
CREATE TABLE Calificaciones (
    IdCalificacion INTEGER PRIMARY KEY AUTOINCREMENT,
    IdUsuario INTEGER NOT NULL,
    IdPelicula INTEGER NOT NULL,
    Puntuacion INTEGER NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    FOREIGN KEY (IdPelicula) REFERENCES Peliculas(IdPelicula)
);


-- RESENAS
-- Parte del proyecto: Reseña opcional de películas.
-- Finalidad: guardar comentarios escritos por los usuarios.
CREATE TABLE Resenas (
    IdResena INTEGER PRIMARY KEY AUTOINCREMENT,
    IdUsuario INTEGER NOT NULL,
    IdPelicula INTEGER NOT NULL,
    Comentario TEXT,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    FOREIGN KEY (IdPelicula) REFERENCES Peliculas(IdPelicula)
);


-- REPRODUCCIONES
-- Parte del proyecto: Reproductor y mini-reproductor persistente.
-- Finalidad: guardar el progreso de reproducción de cada usuario.
CREATE TABLE Reproducciones (
    IdReproduccion INTEGER PRIMARY KEY AUTOINCREMENT,
    IdUsuario INTEGER NOT NULL,
    IdPelicula INTEGER NOT NULL,
    SegundoActual INTEGER DEFAULT 0,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    FOREIGN KEY (IdPelicula) REFERENCES Peliculas(IdPelicula)
);