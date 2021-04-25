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
            { "data": "habitacion.tipoHabitacion.nombre", "width": "15%" },
            { "data": "habitacion.nombre", "width": "15%" },
            { "data": "habitacion.hotel.nombre", "width": "15%" },
            {
                "data": "fechaLlegada", "width": "10%", "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {
                "data": "fechaSalida", "width": "10%", "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            }
            //,
            //{
            //    "data": "reservaId",
            //    "render": function (data) {
            //        return `
            //                <div class="text-center">
            //                    <a onclick="ShowPopup('/Reserva/Upsert/?id=${data}','Actualizar Reserva')" class="btn btn-success text-white" style="cursor:pointer">
            //                        <i class="fas fa-edit"></i> 
            //                    </a>
            //                    <a onclick=Borrar("/Reserva/Borrar/?id=${data}") class="btn btn-danger text-white" style="cursor:pointer">
            //                        <i class="fas fa-trash-alt"></i> 
            //                    </a>
            //                </div>
            //               `;
            //    }, "width": "40%"
            //}
        ],
        "language": {
            "lengthMenu": "Desplegando _MENU_ registros por página",
            "zeroRecords": "Lo sentimos, no se han encontrado registros.",
            "info": "Mostrando página _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros disponibles.",
            "infoFiltered": "(filtrado de _MAX_ registros.)",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Filtrar:",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });
}
