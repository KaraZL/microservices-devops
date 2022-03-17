using MediatR;
using Movies.DataAccess.Commands;
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
    public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, int>
    {
        private readonly IMovieRepo _repo;

        public DeleteMovieHandler(IMovieRepo repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            await _repo.Delete(request.Id);

            return request.Id;
        }
    }
}
