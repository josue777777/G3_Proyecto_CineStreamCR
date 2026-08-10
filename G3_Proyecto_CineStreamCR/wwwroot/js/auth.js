// =========================================================
// LOGIN Y REGISTRO
// Parte del proyecto: autenticación.
//
// Finalidad:
// Controlar las transiciones visuales entre el formulario
// de Inicio de sesión y Registro dentro de la misma vista,
// además del desplazamiento hacia la sección de acceso.
// =========================================================

document.addEventListener("DOMContentLoaded", function () {

    // Contenedor principal que posee las dos caras:
    // Login y Registro.
    const authCard =
        document.getElementById("authCard");

    // Botones ubicados dentro de los formularios.
    const btnMostrarRegistro =
        document.getElementById("btnMostrarRegistro");

    const btnMostrarLogin =
        document.getElementById("btnMostrarLogin");

    // Botones ubicados en el Hero.
    const btnHeroLogin =
        document.getElementById("btnHeroLogin");

    const btnHeroRegister =
        document.getElementById("btnHeroRegister");

    // Botones ubicados en el Header de la pantalla de acceso.
    const btnHeaderLogin =
        document.getElementById("btnHeaderLogin");

    const btnHeaderRegister =
        document.getElementById("btnHeaderRegister");

    // Sección donde están los formularios.
    const seccionAcceso =
        document.getElementById("acceso");

    // Header de la pantalla de acceso.
    const header =
        document.querySelector(".landing-header");


    // =====================================================
    // FUNCIONES
    // =====================================================

    // Muestra visualmente el formulario de Login.
    function mostrarLogin() {

        authCard?.classList.remove("is-register");

        irAFormulario();
    }


    // Muestra visualmente el formulario de Registro.
    function mostrarRegistro() {

        authCard?.classList.add("is-register");

        irAFormulario();
    }


    // Desplaza suavemente la página hasta la
    // sección donde se encuentran los formularios.
    function irAFormulario() {

        seccionAcceso?.scrollIntoView({
            behavior: "smooth",
            block: "center"
        });
    }


    // =====================================================
    // EVENTOS DEL LOGIN
    // =====================================================

    btnMostrarLogin?.addEventListener(
        "click",
        mostrarLogin
    );

    btnHeroLogin?.addEventListener(
        "click",
        mostrarLogin
    );

    btnHeaderLogin?.addEventListener(
        "click",
        mostrarLogin
    );


    // =====================================================
    // EVENTOS DEL REGISTRO
    // =====================================================

    btnMostrarRegistro?.addEventListener(
        "click",
        mostrarRegistro
    );

    btnHeroRegister?.addEventListener(
        "click",
        mostrarRegistro
    );

    btnHeaderRegister?.addEventListener(
        "click",
        mostrarRegistro
    );


    // =====================================================
    // EFECTO DEL HEADER AL HACER SCROLL
    // =====================================================

    window.addEventListener(
        "scroll",
        function () {

            if (window.scrollY > 40) {

                header?.classList.add("scrolled");

            } else {

                header?.classList.remove("scrolled");

            }

        }
    );

});