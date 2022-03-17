using MediatR;
using Movies.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess.Commands
{
    public record DeleteMovieCommand(int Id) : IRequest<int>
    {
    }
}
