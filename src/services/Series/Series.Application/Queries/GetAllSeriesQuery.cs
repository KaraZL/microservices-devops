using MediatR;
using Series.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application.Queries
{
    public record GetAllSeriesQuery : IRequest<IEnumerable<SeriesModel?>?>
    {
    }
}
