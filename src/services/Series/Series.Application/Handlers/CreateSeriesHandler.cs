using MediatR;
using Series.API.Data;
using Series.Application.Commands;
using Series.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application.Handlers
{
    public class CreateSeriesHandler : IRequestHandler<CreateSeriesCommand, SeriesModel>
    {
        private readonly ISeriesRepo _repo;

        public CreateSeriesHandler(ISeriesRepo repo)
        {
            _repo = repo;
        }

        public async Task<SeriesModel> Handle(CreateSeriesCommand request, CancellationToken cancellationToken)
        {
            var series = request.series;

            await _repo.CreateSeries(series);

            return series;
        }
    }
}
