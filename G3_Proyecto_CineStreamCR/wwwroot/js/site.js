
// ESTO ES PARA EL ASIDE DE LA VISTA COMPARTIDA, PARA QUE AL DARLE CLICK A UN BOTÓN, SE PONGA ACTIVO Y LOS DEMÁS NO.

document.addEventListener("DOMContentLoaded", function () {

    const asideItems =
        document.querySelectorAll(".aside-item");

    asideItems.forEach(function (item) {

        item.addEventListener("click", function () {

            // Solo cambia visualmente el estado activo
            // en elementos que todavía no navegan.
            if (item.tagName === "BUTTON") {

                asideItems.forEach(function (element) {
                    element.classList.remove("active");
                });

                item.classList.add("active");
            }

        });

    });

});