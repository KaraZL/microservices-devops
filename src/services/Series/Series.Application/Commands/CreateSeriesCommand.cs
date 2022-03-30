using MediatR;
using Series.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application.Commands
{
    public record CreateSeriesCommand(SeriesModel series) : IRequest<SeriesModel>;
}
