using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Padel.Application.Database;
using Padel.Application.Repositories;
using Padel.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IMatchRepository, MatchRepository>(); // Register MatchRepository first
            services.AddSingleton<ISeasonRepository, SeasonRepository>(); // Then register SeasonRepository
            services.AddSingleton<ITeamRepository, TeamRepository>();
            services.AddSingleton<IPlayerRepository, PlayerRepository>();
            services.AddSingleton<ITeamService, TeamService>();
            services.AddSingleton<IPlayerService, PlayerService>();
            services.AddSingleton<IMatchService, MatchService>();
            services.AddSingleton<ISeasonService, SeasonService>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

            return services;
        }
        //public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        //{
        //    services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        //    services.AddSingleton<DbInitializer>();
        //    return services;
        //}
    }
}
