using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Movies.DataAccess.Behaviors;

namespace Movies.DataAccess
{
    //Permet de recupérer les services de ce projet et de le placer dans un autre
    public static class ApplicationServiceRegistration
    {
        //this, permet d'appeler directement services.AddApplicationServices dans Movies.API
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Add ValidatorsCommand
            services.AddValidatorsFromAssembly(typeof(MediatRAssembly).Assembly);

            //ValidationBehavior
            //Transient pour que la validation soit lancé à chaque requete
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
