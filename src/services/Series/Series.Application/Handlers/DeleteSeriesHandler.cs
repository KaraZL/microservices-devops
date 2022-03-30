using MediatR;
using Series.API.Data;
using Series.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application.Handlers
{
    public class DeleteSeriesHandler : IRequestHandler<DeleteSeriesCommand, Unit>
    {
        private readonly ISeriesRepo _repo;

        public DeleteSeriesHandler(ISeriesRepo repo)
        {
            _repo = repo;
        }
        public async Task<Unit> Handle(DeleteSeriesCommand request, CancellationToken cancellationToken)
        {
            var id = request.id;

            await _repo.DeleteSeries(id);

            return new Unit();
        }
    }
}
