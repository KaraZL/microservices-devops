using FluentValidation;
using Series.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application.Behaviors
{
    public class CreateSeriesCommandValidator : AbstractValidator<CreateSeriesCommand>
    {
        public CreateSeriesCommandValidator()
        {
            RuleFor(x => x.series.Id).NotEmpty();
            RuleFor(x => x.series.Name).NotEmpty();
            RuleFor(x => x.series.CreatedBy).NotEmpty();
            RuleFor(x => x.series.CountryOrigin.CountryName).NotEmpty();
            RuleFor(x => x.series.CountryOrigin.ZipCode).NotEmpty();
        }
    }
}
