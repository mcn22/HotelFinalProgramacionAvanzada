var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblTiposHabitacion').DataTable({
        "ajax": {
            "url": "/TipoHabitacion/Listar"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            { "data": "descripcion", "width": "15%" },
            {
                "data": "costoNoche",
                "render": function (data) {
                    return `
                           ₡ ${data}
                           `;
                }, "width": "15%" },

            {
                "data": "tipoHabitacionId",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="ShowPopup('/TipoHabitacion/Upsert/?id=${data}','Actualizar Habitacion')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Borrar("/TipoHabitacion/Borrar/?id=${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}
