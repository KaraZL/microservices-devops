using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application.Commands
{
    public record DeleteSeriesCommand(string id) : IRequest<Unit>; //Unit is void similar
}
