var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblHoteles').DataTable({
        "ajax": {
            "url": "/Hotel/Listar"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
            { "data": "descripcion", "width": "15%" },
            { "data": "direccion", "width": "15%" },
            { "data": "ciudad", "width": "15%" },
            { "data": "telefono", "width": "15%" },
            {
                "data": "hotelId",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick="ShowPopup('/Hotel/Upsert/?id=${data}','Actualizar Hotel')" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Borrar("/Hotel/Borrar/?id=${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}
