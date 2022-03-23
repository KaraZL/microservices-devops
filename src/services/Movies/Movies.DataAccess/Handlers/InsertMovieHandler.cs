using AutoMapper;
using MediatR;
using Movies.DataAccess.Commands;
using Movies.DataAccess.Dtos;
using Movies.DataAccess.Models;
using Movies.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.DataAccess.Handlers
{
    internal class InsertMovieHandler : IRequestHandler<InsertMovieCommand, Movie>
    {
        private readonly IMovieRepo _repo;
        private readonly IMapper _mapper;

        public InsertMovieHandler(IMovieRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<Movie> Handle(InsertMovieCommand request, CancellationToken cancellationToken)
        {
            //var movie = new Movie
            //{
            //    Name = request.Name,
            //    Author = request.Author,
            //    Year = request.Year,
            //};

            //Id est auto incrémenté, il ne doit pas être dedans, on utilise alors Mapper
            var movie = _mapper.Map<MovieDatabaseDto>(request.movie);

            await _repo.Add(request.movie);

            return request.movie;
        }
    }
}
