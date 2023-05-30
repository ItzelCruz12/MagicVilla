using AutoMapper;
using MagicVillaApi.Datos;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dto;
using MagicVillaApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        //private readonly ApplicationDbContext _Db;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _Mapper;
        protected APIResponse _response;
        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, INumeroVillaRepositorio numeroRepo, IMapper mapper)
        {
            //Inicializamos
            _logger = logger;
            _villaRepo = villaRepo;
            _numeroRepo = numeroRepo;
            _Mapper = mapper;
            _response = new();
        }
        //[HttpGet]
        //public IEnumerable<VillaDto> GetVillas()
        //{
        //    return VillaStore.VillaList;
        //}
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //Implementacion de la interfaz en actionResult
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener Numero villas");
                IEnumerable<NumeroVilla> numerovillaList = await _numeroRepo.ObtenerTodos();
                _response.Resultado = _Mapper.Map<IEnumerable<NumeroVillaDto>>(numerovillaList);
                _response.statusCode = HttpStatusCode.OK;
                //return Ok(VillaStore.VillaList);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("id:int", Name = "GetNumeroVilla")]
        //En esta parte se documentan los errores de estado esas son 2 formas de hacerlo.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                //Validamos si se encuentra el id con los diferentes codigos
                if (id == 0)
                {
                    _logger.LogError("Error al traer numero villa con Id " + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso=false;
                    return BadRequest(_response);
                }
                //var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
                var numerovilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
                if (numerovilla == null)
                {
                    _response.statusCode=HttpStatusCode.NotFound;
                    _response.IsExitoso=false;
                    return NotFound(_response);
                }
                _response.Resultado = _Mapper.Map<NumeroVillaDto>(numerovilla);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };              
            }
            return _response;
            
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
        public async Task<ActionResult<APIResponse>>CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                //ModelState es valido pero si se l epone el signo ! es que sea diferente de valido
                //entonces retorna un badRequest y te dice que es lo que esta mal escrito
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe!");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("Clave Foranea", "El id de la  villa con ese nombre No existe!");
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
                NumeroVilla modelo = _Mapper.Map<NumeroVilla>(createDto);
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
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _numeroRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;
                //await _Db.SaveChangesAsync();

                //villaDto.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                //VillaStore.VillaList.Add(villaDto);
                // return Ok(villaDto);
                return CreatedAtRoute("GetVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string> { ex.ToString()};
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso= false;
                    _response.statusCode= HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var numerovilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
                if (numerovilla == null)
                {
                    _response.IsExitoso= false;
                    _response.statusCode= HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _numeroRepo.Remove(numerovilla);
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.VillaNo)
            {
                _response.IsExitoso= false;
                _response.statusCode= HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if(await _villaRepo.Obtener(V=> V.Id== updateDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El id de la villa no existe");
                return BadRequest(ModelState);
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            //SE ocupa para el mapeo 
            NumeroVilla modelo = _Mapper.Map<NumeroVilla>(updateDto);

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
            await _numeroRepo.Actualizar(modelo);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);

        }

        //[HttpPatch("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        //{
        //    if (patchDto == null || id == 0)
        //    {
        //        return BadRequest();
        //    }
        //    //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
        //    //Se agrego AsNoTracking(). para qu eno se confundiera visual sobre que argumentos utilizar
        //    var villa = await _villaRepo.Obtener(v => v.Id == id, tracked: false);

        //    ////Se agrega para el mapeo
        //    VillaUpdateDto villaDto = _Mapper.Map<VillaUpdateDto>(villa);

        //    //VillaUpdateDto villaDto = new()
        //    //{
        //    //    Id = villa.Id,
        //    //    Nombre = villa.Nombre,
        //    //    Detalle = villa.Detalle,
        //    //    ImagenUrl = villa.ImagenUrl,
        //    //    Ocupantes = villa.Ocupantes,
        //    //    Tarifa = villa.Tarifa,
        //    //    MetrosCuadrados = villa.MetrosCuadrados,
        //    //    Amenidad = villa.Amenidad
        //    //};
        //    if (villa == null) return BadRequest();

        //    patchDto.ApplyTo(villaDto, ModelState);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    //Se ocupa para el mapeo
        //    Villa modelo = _Mapper.Map<Villa>(villaDto);

        //    //Villa modelo = new()
        //    //{
        //    //    Id = villaDto.Id,
        //    //    Nombre = villaDto.Nombre,
        //    //    Detalle = villaDto.Detalle,
        //    //    ImagenUrl = villaDto.ImagenUrl,
        //    //    Ocupantes = villaDto.Ocupantes,
        //    //    Tarifa = (int)villaDto.Tarifa,
        //    //    MetrosCuadrados = villaDto.MetrosCuadrados,
        //    //    Amenidad = villaDto.Amenidad
        //    //};
        //    await _villaRepo.Actualizar(modelo);
        //    _response.statusCode = HttpStatusCode.NoContent;
        //    return Ok(_response);
        //}
    }
} 

