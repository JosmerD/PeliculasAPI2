using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.DTOs
{
    public class CinesCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }
        [Range(-90,90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }
    }
}
