using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Movies.DataAccess.Dtos;
using Movies.DataAccess.Models;

namespace Movies.DataAccess.Profiles
{
    //Id est auto incrémenté, il ne doit pas être dedans, on utilise alors Mapper
    //Notamment pour InsertMovieHandler
    internal class MovieProfiles : Profile
    {
        public MovieProfiles()
        {
            CreateMap<Movie, MovieDatabaseDto>();
        }
    }
}
