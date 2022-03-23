using FluentValidation;
using Movies.DataAccess.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DataAccess.Behaviors
{
    public class InsertMovieCommandValidator : AbstractValidator<InsertMovieCommand>
    {
        public InsertMovieCommandValidator()
        {
            //Pas besoin de tester Id parce qu'il n'est pas pris en compte grace au Dto
            RuleFor(x => x.movie.Name).NotEmpty();
            RuleFor(x => x.movie.Author).NotEmpty();
            RuleFor(x => x.movie.Year).NotEmpty();

            RuleFor(x => x.movie.Year).GreaterThan(0).LessThan(3000);
            RuleFor(x => x.movie.Name).Length(4, 30);
            RuleFor(x => x.movie.Author).Must(BeValidAuthor).WithMessage("Please enter a correct Author starting with Zaf");
        }

        //Ajouter une validation perso
        //Nom doit comporter "zaf"
        private bool BeValidAuthor(string author)
        {
            author = author.ToUpper();
            return author.Contains("ZAF");
        }
    }
}
