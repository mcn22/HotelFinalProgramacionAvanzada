var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblHabitaciones').DataTable({
        "ajax": {
            "url": "/Habitacion/Listar"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            { "data": "tipoHabitacion.nombre", "width": "15%" },
            { "data": "hotel.nombre", "width": "15%" },
            {
                "data": "habitacionId",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="ShowPopup('/Habitacion/Upsert/?id=${data}','Actualizar Habitacion')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Borrar("/Habitacion/Borrar/?id=${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}
