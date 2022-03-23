using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.DataAccess.Behaviors
{
    //error CS0314 sans where constraints
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        //Recuperer tous les etats de validators en cours
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //RequestHandlerDelegate<TResponse> next
            //Permet de faire avancer le pipeline comme dans pattern.txt

            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(x => x.Validate(context)) //Validate context
                .SelectMany(x => x.Errors) //get errors
                .Where(x => x != null) //Si ils sont pas nulles
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            //avance dans le pipeline -> handler
            return await next();

        }
    }
}
