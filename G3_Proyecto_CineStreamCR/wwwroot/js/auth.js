document.addEventListener("DOMContentLoaded", function () {

    const authCard =
        document.getElementById("authCard");

    const btnMostrarRegistro =
        document.getElementById("btnMostrarRegistro");

    const btnMostrarLogin =
        document.getElementById("btnMostrarLogin");


    // Cambia visualmente al formulario de registro.
    btnMostrarRegistro?.addEventListener(
        "click",
        function () {
            authCard.classList.add("is-register");
        }
    );


    // Regresa visualmente al formulario de inicio de sesión.
    btnMostrarLogin?.addEventListener(
        "click",
        function () {
            authCard.classList.remove("is-register");
        }
    );

});