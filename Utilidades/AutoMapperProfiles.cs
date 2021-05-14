using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NetTopologySuite.Geometries;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;

namespace PeliculasApi.Utilidades
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(x=>x.Foto,options=>options.Ignore());

            CreateMap<CinesCreacionDTO, Cine>().ForMember(x=>x.Ubicacion,x=>x.MapFrom(dto=> geometryFactory.CreatePoint(new Coordinate(dto.Longitud,dto.Latitud))));
        }
    }
}
