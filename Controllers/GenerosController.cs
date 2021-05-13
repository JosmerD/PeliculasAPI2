using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Filtros;
using PeliculasApi.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeliculasApi.Utilidades;

namespace PeliculasApi.Controllers
{
    [Route("api/generos")]
    [ApiController]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController:ControllerBase
    {
        //private readonly IRepositorio repositorio;
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        // public GenerosController(IRepositorio repositorio, ILogger<GenerosController> logger) se ocupaba cuando se usaba un repositorio para el curso
        public GenerosController(ILogger<GenerosController> logger, ApplicationDbContext context, IMapper mapper)
        {
          
//            this.repositorio = repositorio;
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }


        //[HttpGet("listado")] //la url queda api/genero/listado
        //[HttpGet("/listadogeneros")] //la url queda /listadogenero
        //[ResponseCache(Duration =60)]
        //[ServiceFilter(typeof(MiFiltroDeAccion))]
        [HttpGet] //la url queda api/genero
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            //logger.LogInformation("vamos a mostrar los generos");
            var queryable = context.Generos.AsQueryable();
            
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<GeneroDTO>>(generos);



        }
        //[HttpGet("{Id:int}/{nombre=Josmer}")]
        [HttpGet("{Id:int}")]
        //public Genero Get(int Id,string nombre) //se coloca actionresult para que retorne un 404, no se puede crear un metodo de una clase para retornar un 404
        //BindRequired valid que el parametro recibido sea requerido y deje un mjs de error
        public async Task<ActionResult<GeneroDTO>> Get(int Id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);
            if (genero==null)
            {
                return NotFound();
            }
            return mapper.Map<GeneroDTO>(genero);
            
        }   
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id,[FromBody] GeneroCreacionDTO generoCreacionDTO)
        {

            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);
            if (genero == null)
            {
                return NotFound();
            }
            genero = mapper.Map(generoCreacionDTO, genero);
            await context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Genero() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
