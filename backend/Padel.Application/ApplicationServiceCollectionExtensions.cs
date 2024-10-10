using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<ITeamRepository , TeamRepository>();
            services.AddSingleton<IPlayerRepository , PlayerRepository>();
            services.AddSingleton<ITeamService, TeamService>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

            return services;
        }
    }
}
