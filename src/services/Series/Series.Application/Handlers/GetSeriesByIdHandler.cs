using MediatR;
using Series.API.Data;
using Series.Application.Queries;
using Series.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application.Handlers
{
    public class GetSeriesByIdHandler : IRequestHandler<GetSeriesByIdQuery, SeriesModel?>
    {
        private readonly ISeriesRepo _repo;

        public GetSeriesByIdHandler(ISeriesRepo repo)
        {
            _repo = repo;
        }
        public async Task<SeriesModel?> Handle(GetSeriesByIdQuery request, CancellationToken cancellationToken)
        {
            var id = request.id;

            var series = await _repo.GetSeriesById(id);

            return series;
        }
    }
}
