using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/cines")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class CinesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CinesController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet] //la url queda api/genero
        public async Task<ActionResult<List<cineDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            //logger.LogInformation("vamos a mostrar los generos");
            var queryable = context.Cines.AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var cines = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<cineDTO>>(cines);



        }
        [HttpGet("{Id:int}")]
        public async Task<ActionResult<cineDTO>> Get(int Id)
        {
            var cines = await context.Cines.FirstOrDefaultAsync(x => x.Id == Id);
            if (cines == null)
            {
                return NotFound();
            }
            return mapper.Map<cineDTO>(cines);

        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CinesCreacionDTO cinesCreacionDTO)
        {
            var actor = mapper.Map<Cine>(cinesCreacionDTO);
       
            context.Add(actor);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] CinesCreacionDTO cinesCreacionDTO)
        {

            var cines = await context.Cines.FirstOrDefaultAsync(x => x.Id == Id);
            if (cines == null)
            {
                return NotFound();
            }
            cines = mapper.Map(cinesCreacionDTO, cines);
            await context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Cines.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Cine() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
