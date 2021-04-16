var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblReservas').DataTable({
        "ajax": {
            "url": "/Reserva/Listar"
        },
        "columns": [
            { "data": "reservaId", "width": "5%" },
            { "data": "usuario.nombre", "width": "15%" },
            { "data": "habitacion.hotel.nombre", "width": "15%" },
            {
                "data": "reservaId",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="ShowPopup('/Reserva/Upsert/?id=${data}','Actualizar Reserva')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Borrar("/Reserva/Borrar/?id=${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}
