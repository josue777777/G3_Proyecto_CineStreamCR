-- =========================================================
-- BASE DE DATOS: CineStream CR
--
-- IMPORTANTE:
-- Este archivo documenta el diseño esperado de la base de datos.
-- El esquema real de la aplicación se crea y mantiene mediante
-- Entity Framework Core y migraciones.
--
-- No ejecutar este script manualmente para modificar la base
-- durante el desarrollo normal.
-- =========================================================


-- =========================================================
-- USUARIOS
-- Parte del proyecto:
-- Login, WatchLists, calificaciones y reproducción.
--
-- Finalidad:
-- Guardar los datos básicos del usuario autenticado.
-- =========================================================
CREATE TABLE Usuarios (
    IdUsuario INTEGER PRIMARY KEY AUTOINCREMENT,

    NombreUsuario TEXT NOT NULL UNIQUE,

    Correo TEXT NOT NULL UNIQUE,

    -- La contraseña no debe almacenarse en texto plano.
    PasswordHash TEXT NOT NULL
);


-- =========================================================
-- PERSONAS
-- Parte del proyecto:
-- Perfil de directores y actores.
--
-- Finalidad:
-- Representar tanto directores como actores en una única tabla,
-- evitando duplicar información común.
-- =========================================================
CREATE TABLE Personas (
    IdPersona INTEGER PRIMARY KEY AUTOINCREMENT,

    Nombre TEXT NOT NULL,

    Nacionalidad TEXT,

    Biografia TEXT,

    FechaNacimiento TEXT,

    FotoUrl TEXT
);


-- =========================================================
-- GENEROS
-- Parte del proyecto:
-- Catálogo, filtros y clasificación de películas.
-- =========================================================
CREATE TABLE Generos (
    IdGenero INTEGER PRIMARY KEY AUTOINCREMENT,

    Nombre TEXT NOT NULL UNIQUE
);


-- =========================================================
-- PELICULAS
-- Parte del proyecto:
-- Catálogo, detalle y reproducción.
--
-- Finalidad:
-- Guardar la información principal de cada película.
-- =========================================================
CREATE TABLE Peliculas (
    IdPelicula INTEGER PRIMARY KEY AUTOINCREMENT,

    Titulo TEXT NOT NULL,

    Sinopsis TEXT,

    Anio INTEGER NOT NULL,

    DuracionMinutos INTEGER NOT NULL,

    PosterUrl TEXT,

    VideoUrl TEXT,

    Rating REAL NOT NULL DEFAULT 0,

    -- El director también pertenece a la tabla Personas.
    IdDirector INTEGER NOT NULL,

    FOREIGN KEY (IdDirector)
        REFERENCES Personas(IdPersona)
);


-- =========================================================
-- PELICULA_GENEROS
-- Parte del proyecto:
-- Filtro por género y detalle de película.
--
-- Finalidad:
-- Permitir que una película tenga uno o varios géneros.
-- =========================================================
CREATE TABLE PeliculaGeneros (
    IdPelicula INTEGER NOT NULL,

    IdGenero INTEGER NOT NULL,

    PRIMARY KEY (IdPelicula, IdGenero),

    FOREIGN KEY (IdPelicula)
        REFERENCES Peliculas(IdPelicula),

    FOREIGN KEY (IdGenero)
        REFERENCES Generos(IdGenero)
);


-- =========================================================
-- PELICULA_ACTORES
-- Parte del proyecto:
-- Elenco principal y perfil del actor.
--
-- Finalidad:
-- Relacionar películas con actores y almacenar el personaje
-- interpretado.
--
-- IdActor referencia también a Personas.
-- =========================================================
CREATE TABLE PeliculaActores (
    IdPelicula INTEGER NOT NULL,

    IdActor INTEGER NOT NULL,

    Personaje TEXT NOT NULL,

    PRIMARY KEY (IdPelicula, IdActor),

    FOREIGN KEY (IdPelicula)
        REFERENCES Peliculas(IdPelicula),

    FOREIGN KEY (IdActor)
        REFERENCES Personas(IdPersona)
);


-- =========================================================
-- WATCHLISTS
-- Parte del proyecto:
-- Gestión de listas personalizadas.
--
-- Finalidad:
-- Permitir que cada usuario cree listas propias de películas.
-- =========================================================
CREATE TABLE WatchLists (
    IdWatchList INTEGER PRIMARY KEY AUTOINCREMENT,

    Nombre TEXT NOT NULL,

    Descripcion TEXT,

    IdUsuario INTEGER NOT NULL,

    FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(IdUsuario),

    -- Evita que el mismo usuario tenga dos listas con
    -- exactamente el mismo nombre.
    UNIQUE (IdUsuario, Nombre)
);


-- =========================================================
-- WATCHLIST_PELICULAS
-- Parte del proyecto:
-- Agregar y quitar películas de una WatchList.
--
-- Finalidad:
-- Relacionar las WatchLists con las películas.
-- =========================================================
CREATE TABLE WatchListPeliculas (
    IdWatchList INTEGER NOT NULL,

    IdPelicula INTEGER NOT NULL,

    PRIMARY KEY (IdWatchList, IdPelicula),

    FOREIGN KEY (IdWatchList)
        REFERENCES WatchLists(IdWatchList),

    FOREIGN KEY (IdPelicula)
        REFERENCES Peliculas(IdPelicula)
);


-- =========================================================
-- CALIFICACIONES
-- Parte del proyecto:
-- Calificación y reseña opcional de una película.
--
-- Finalidad:
-- Guardar la valoración realizada por un usuario.
-- =========================================================
CREATE TABLE Calificaciones (
    IdCalificacion INTEGER PRIMARY KEY AUTOINCREMENT,

    IdUsuario INTEGER NOT NULL,

    IdPelicula INTEGER NOT NULL,

    -- El enunciado solicita puntuación de 1 a 10.
    Puntuacion INTEGER NOT NULL
        CHECK (Puntuacion >= 1 AND Puntuacion <= 10),

    -- La reseña es opcional.
    Resena TEXT,

    FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(IdUsuario),

    FOREIGN KEY (IdPelicula)
        REFERENCES Peliculas(IdPelicula),

    -- Un usuario mantiene como máximo una calificación
    -- por película.
    UNIQUE (IdUsuario, IdPelicula)
);


-- =========================================================
-- REPRODUCCIONES
-- Parte del proyecto:
-- Reproductor y mini-reproductor persistente.
--
-- Finalidad:
-- Guardar el progreso actual de reproducción de cada usuario
-- para cada película.
-- =========================================================
CREATE TABLE Reproducciones (
    IdReproduccion INTEGER PRIMARY KEY AUTOINCREMENT,

    IdUsuario INTEGER NOT NULL,

    IdPelicula INTEGER NOT NULL,

    SegundoActual INTEGER NOT NULL DEFAULT 0,

    FechaUltimaReproduccion TEXT,

    FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(IdUsuario),

    FOREIGN KEY (IdPelicula)
        REFERENCES Peliculas(IdPelicula),

    -- Un usuario mantiene un único progreso por película.
    UNIQUE (IdUsuario, IdPelicula)
);