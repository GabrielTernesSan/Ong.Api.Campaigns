using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Ong.Application.Behaviors;

namespace Ong.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly));

            //services.AddValidatorsFromAssembly(typeof(ApplicationServiceCollectionExtensions).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
