using MagicVillaApi.Datos;
using MagicVillaApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _Db;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext Db)
        {
            _logger = logger;
            _Db = Db;
        }
        //[HttpGet]
        //public IEnumerable<VillaDto> GetVillas()
        //{
        //    return VillaStore.VillaList;
        //}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //Implementacion de la interfaz en actionResult
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
            //return Ok(VillaStore.VillaList);
            return Ok(_Db.Villas.ToList());
        }

        [HttpGet("id:int", Name = "GetVilla")]
        //En esta parte se documentan los errores de estado esas son 2 formas de hacerlo.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            //Validamos si se encuentra el id con los diferentes codigos
            if (id == 0)
            {
                _logger.LogError("Error al traer villa con Id " + id);
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            var villa = _Db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }
        //////Identificar o diferenciar los EndPoint 
        //[HttpGet("id")]
        //public VillaDto GetVilla(int id)
        //{
        //////se le reortna de la villastore la lista que creamos por una expresion lamda con el 
        //////FirstOrDefault y se le da nombre de v y con el => se llama al ID y se le dice que sea == a id ingresado
        //    return VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto)
        {
            //ModelState es valido pero si se l epone el signo ! es que sea diferente de valido
            //entonces retorna un badRequest y te dice que es lo que esta mal escrito
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_Db.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa modelo = new()
            {
                //no se necesita pasar el id porque es automatico
               // Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = (int)villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };
            _Db.Villas.Add(modelo);
            _Db.SaveChanges();

            //villaDto.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //VillaStore.VillaList.Add(villaDto);
            // return Ok(villaDto);
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _Db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _Db.Villas.Remove(villa);
            _Db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = (int)villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };
            _Db.Villas.Update(modelo);
            _Db.SaveChanges();
            return NoContent();

        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            var villa = _Db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);
            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = (int)villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };
            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = (int)villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };
            _Db.Villas.Update(modelo);
            _Db.SaveChanges();
            return NoContent();
        }

    }
} 

