using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.DTOs
{
    public class PeliculasPutGetDTO
    {
        public PeliculaDTO Pelicula { get; set; }
        public List<GeneroDTO> GenerosSeleccionados { get; set; }
        public List<GeneroDTO> GenerosNoSeleccionados { get; set; }
        public List<cineDTO> CinesSeleccionados { get; set; }
        public List<cineDTO> CinesNoSeleccionados { get; set; }
        public List<PeliculaActorDTO> Actores { get; set; }
    }
}
