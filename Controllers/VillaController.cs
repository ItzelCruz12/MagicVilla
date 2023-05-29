using AutoMapper;
using MagicVillaApi.Datos;
using MagicVillaApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _Db;
        private readonly IMapper _Mapper;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext Db, IMapper mapper)
        {
            _logger = logger;
            _Db = Db;
            _Mapper = mapper;
        }
        //[HttpGet]
        //public IEnumerable<VillaDto> GetVillas()
        //{
        //    return VillaStore.VillaList;
        //}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //Implementacion de la interfaz en actionResult
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
            IEnumerable<Villa> villaList = await _Db.Villas.ToListAsync();
            //return Ok(VillaStore.VillaList);
            return Ok(_Mapper.Map<IEnumerable<VillaDto>>(villaList));
        }

        [HttpGet("id:int", Name = "GetVilla")]
        //En esta parte se documentan los errores de estado esas son 2 formas de hacerlo.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            //Validamos si se encuentra el id con los diferentes codigos
            if (id == 0)
            {
                _logger.LogError("Error al traer villa con Id " + id);
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            var villa = await _Db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_Mapper.Map<VillaDto>(villa));
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
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            //ModelState es valido pero si se l epone el signo ! es que sea diferente de valido
            //entonces retorna un badRequest y te dice que es lo que esta mal escrito
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _Db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }
            if (createDto == null)
            {
                return BadRequest(createDto);
            }
            //if (villaDto.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            ////
            ///Este se crea para usar el MAPER
            Villa modelo = _Mapper.Map<Villa>(createDto);
            //Villa modelo = new()
            //{
            //    //no se necesita pasar el id porque es automatico
            //   // Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = (int)villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad
            //};
            await _Db.Villas.AddAsync(modelo);
            await _Db.SaveChangesAsync();

            //villaDto.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //VillaStore.VillaList.Add(villaDto);
            // return Ok(villaDto);
            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _Db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _Db.Villas.Remove(villa);
            await _Db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            //SE ocupa para el mapeo 
            Villa modelo = _Mapper.Map<Villa>(updateDto);

            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = (int)villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad
            //};
            _Db.Villas.Update(modelo);
            await _Db.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            //Se agrego AsNoTracking(). para qu eno se confundiera visual sobre que argumentos utilizar
            var villa = await _Db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            ////Se agrega para el mapeo
            VillaUpdateDto villaDto = _Mapper.Map<VillaUpdateDto>(villa);

            //VillaUpdateDto villaDto = new()
            //{
            //    Id = villa.Id,
            //    Nombre = villa.Nombre,
            //    Detalle = villa.Detalle,
            //    ImagenUrl = villa.ImagenUrl,
            //    Ocupantes = villa.Ocupantes,
            //    Tarifa = villa.Tarifa,
            //    MetrosCuadrados = villa.MetrosCuadrados,
            //    Amenidad = villa.Amenidad
            //};
            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Se ocupa para el mapeo
            Villa modelo = _Mapper.Map<Villa>(villaDto);

            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = (int)villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad
            //};
            _Db.Villas.Update(modelo);
            await _Db.SaveChangesAsync();
            return NoContent();
        }

    }
} 

