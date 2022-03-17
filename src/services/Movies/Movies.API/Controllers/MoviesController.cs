using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Movies.DataAccess.Commands;
using Movies.DataAccess.Models;
using Movies.DataAccess.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            var data = await _mediator.Send(new GetAllMoviesQuery());
            return Ok(data);
        }

         [HttpPost("[action]")]
         public IActionResult AddMovie(Movie movie)
        {
            _mediator.Send(new InsertMovieCommand(movie));
            return Ok();
        }

    }
}
