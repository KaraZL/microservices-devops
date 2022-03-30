using FluentValidation;
using Series.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application.Behaviors
{
    public class DeleteSeriesCommandValidator : AbstractValidator<DeleteSeriesCommand>
    {
        public DeleteSeriesCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty();
        }
    }
}
