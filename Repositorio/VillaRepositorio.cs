using MagicVillaApi.Datos;
using MagicVillaApi.Models;
using MagicVillaApi.Repositorio.IRepositorio;
using Microsoft.IdentityModel.Tokens;

namespace MagicVillaApi.Repositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly ApplicationDbContext _Db;

        public VillaRepositorio(ApplicationDbContext Db) : base(Db)
        {
            _Db= Db;
        }

        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _Db.Villas.Update(entidad);
            await _Db.SaveChangesAsync();
            return entidad;
        }
    }
}
