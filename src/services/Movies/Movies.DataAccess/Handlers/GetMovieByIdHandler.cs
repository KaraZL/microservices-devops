using MediatR;
using Movies.DataAccess.Models;
using Movies.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.DataAccess.Handlers
{
    public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, Movie>
    {
        private readonly IMediator _mediator;

        public GetMovieByIdHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Movie> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var allMovies = await _mediator.Send(new GetAllMoviesQuery());

            var output = allMovies.FirstOrDefault(x => x.Id == request.id);

            return output;
        }
    }
}
