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
    public class GetAllSeriesHandler : IRequestHandler<GetAllSeriesQuery, IEnumerable<SeriesModel?>?>
    {
        private readonly ISeriesRepo _repo;

        public GetAllSeriesHandler(ISeriesRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SeriesModel?>?> Handle(GetAllSeriesQuery request, CancellationToken cancellationToken)
        {
            var series = await _repo.GetAllSeries();

            return series;
        }
    }
}
