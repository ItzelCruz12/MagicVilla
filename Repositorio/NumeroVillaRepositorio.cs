using MagicVillaApi.Datos;
using MagicVillaApi.Models;
using MagicVillaApi.Repositorio.IRepositorio;

namespace MagicVillaApi.Repositorio
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    {   
        private readonly ApplicationDbContext _Db;

        public NumeroVillaRepositorio(ApplicationDbContext Db) : base(Db)
        {
            _Db= Db;
        }

        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _Db.NumeroVillas.Update(entidad);
            await _Db.SaveChangesAsync();
            return entidad;
        }
    }
}
