using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ong.Domain.Repositories;
using Ong.Domain.Repositories.UnitOfWork;
using Ong.Domain.Services;
using Ong.Infra.Repositories;
using Ong.Infra.Repositories.UnitOfWork;
using Ong.Infra.Services;

namespace Ong.Infra
{
    public static class InfraServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OngDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDonationRepository, DonationRepository>();
            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

        public static IServiceCollection AddInfraLayerForWorker(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OngDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDonationRepository, DonationRepository>();
            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();

            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
