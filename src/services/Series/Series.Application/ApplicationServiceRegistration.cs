using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Series.Application.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Series.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Add Validators - FluentValidation;
            services.AddValidatorsFromAssembly(typeof(MediatRAssembly).Assembly);

            //ValidationBehavior
            //Transient pour que la validation soit lancé à chaque requete
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
