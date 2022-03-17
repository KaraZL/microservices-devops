using MediatR;
using Movies.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess.Queries
{
    public record GetAllMoviesQuery : IRequest<List<Movie>>
    {
    }
}
