using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext context,IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>>Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable =  context.Actores.AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var actores = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();


            return mapper.Map<List<ActorDTO>>(actores);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.Foto!=null)
            {
                actor.Foto = await almacenadorArchivos.GuardarArchivo(contenedor, actorCreacionDTO.Foto);
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] ActorCreacionDTO actorCreacionDTO)
        {

            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);
            if (genero == null)
            {
                return NotFound();
            }
            genero = mapper.Map(actorCreacionDTO, genero);
            await context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Actores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
