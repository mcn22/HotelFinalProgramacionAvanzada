using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        //ICategoriaRepositorio Categorias { get; }

        //ICubiertaRepositorio Cubiertas { get; }

        //ILibroRepositorio Libros { get; }

        void Guardar();
    }
}
