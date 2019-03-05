using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using LibCardApp.Dtos;
using LibCardApp.Models;

namespace LibCardApp.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Patron, PatronDto>();
            Mapper.CreateMap<PatronDto, Patron>();
        } 
    }
}