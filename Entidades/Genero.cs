﻿using PeliculasApi.Repositorio.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:50,ErrorMessage ="El maximo de caracteres es de 50")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    
    }
}