using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio
{
    public enum Procedimientos
    {
        Buscar,
        Listar,
        Crear,
        Actualizar,
        Borrar
    }

    public interface IProcedimientoAlmacenado : IDisposable
    {
        T Valor<T>(Procedimientos procedimiento, IDictionary<string, object> parametros = null);

        T Buscar<T>(IDictionary<string, object> parametros = null);

        void Ejecutar(string nombreProcedimiento, IDictionary<string, object> parametros = null);

        IEnumerable<T> Listar<T>(IDictionary<string, object> parametros = null);

        Tuple<IEnumerable<T1>, IEnumerable<T2>> Listar<T1, T2>(IDictionary<string, object> parametros = null);

    }
}
