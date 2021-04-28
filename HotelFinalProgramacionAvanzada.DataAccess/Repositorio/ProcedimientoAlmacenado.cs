using HotelFinalProgramacionAvanzada.DataAccess.Data;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.Extension;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio
{
    namespace Extension
    {
        public static class Extension
        {
            public static DynamicParameters Transformar(this IDictionary<string, object> diccionario)
            {
                DynamicParameters parametros = new DynamicParameters();
                foreach (KeyValuePair<string, object> keyValue in diccionario)
                    parametros.Add(keyValue.Key, keyValue.Value);
                return parametros;
            }
        }
    }

    public class ProcedimientoAlmacenado : IProcedimientoAlmacenado
    {
        public ProcedimientoAlmacenado(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        readonly ApplicationDbContext _db;

        static string ConnectionString = "";

        public void Dispose()
        {
            _db.Dispose();
        }

        IEnumerable<T> Listar<T>(string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(nombreProcedimiento, parametros, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        Tuple<IEnumerable<T1>, IEnumerable<T2>> Listar<T1, T2>(string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var result = SqlMapper.QueryMultiple(sqlCon, nombreProcedimiento, parametros, commandType: System.Data.CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();


                if (item1 != null && item2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }

            }

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        T Buscar<T>(string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var value = sqlCon.Query<T>(nombreProcedimiento, parametros, commandType: System.Data.CommandType.StoredProcedure);
                return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            }
        }

        T Valor<T>(string nombreProcedimiento, DynamicParameters parametros = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                return (T)Convert.ChangeType(sqlCon.ExecuteScalar<T>(nombreProcedimiento, parametros, commandType: System.Data.CommandType.StoredProcedure), typeof(T));
            }
        }

        string BuscarNombreProcedimiento<T>(Procedimientos procedimiento)
        {
            FieldInfo atributo = typeof(T).GetField(procedimiento.ToString(), BindingFlags.NonPublic | BindingFlags.Static);
            return atributo.GetValue(null).ToString();
        }

        public T Valor<T>(Procedimientos procedimiento, IDictionary<string, object> parametros = null)
        {
            return Valor<T>(BuscarNombreProcedimiento<T>(procedimiento), parametros?.Transformar());
        }

        public void Ejecutar(string nombreProcedimiento, IDictionary<string, object> parametros = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(nombreProcedimiento, parametros?.Transformar(), commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public T Buscar<T>(IDictionary<string, object> parametros = null)
        {
            return Buscar<T>(BuscarNombreProcedimiento<T>(Procedimientos.Buscar), parametros?.Transformar());
        }

        public IEnumerable<T> Listar<T>(IDictionary<string, object> parametros = null)
        {
            return Listar<T>(BuscarNombreProcedimiento<T>(Procedimientos.Listar), parametros?.Transformar());
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> Listar<T1, T2>(IDictionary<string, object> parametros = null)
        {
            return Listar<T1, T2>(BuscarNombreProcedimiento<T1>(Procedimientos.Listar), parametros?.Transformar());
        }
    }
}
