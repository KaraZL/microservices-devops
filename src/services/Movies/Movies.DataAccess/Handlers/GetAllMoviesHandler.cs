using MediatR;
using Movies.DataAccess.Models;
using Movies.DataAccess.Queries;
using Movies.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.DataAccess.Handlers
{
    public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, List<Movie>>
    {
        private readonly IMovieRepo _repo;

        public GetAllMoviesHandler(IMovieRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<Movie>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetAll();
            return data.ToList();
        }
    }
}
