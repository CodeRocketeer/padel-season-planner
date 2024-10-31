using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Padel.Infrastructure;
using Padel.Application.Rules;
using Padel.Application.Services;
using Padel.Application.Services.Interfaces;
using Padel.Infrastructure.Repositories.Interfaces;
using Padel.Infrastructure.Repositories;


namespace Padel.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISeasonRepository, SeasonRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IMatchService, MatchService>();        
        services.AddScoped<ISeasonService, SeasonService>();
        services.AddScoped<IPlayerService, PlayerService>();
        //services.AddScoped<ISeederService, SeederService>();


        // Registering rules as transient (or singleton, depending on your needs)
        services.AddTransient<IRule, ConsecutiveParticipantsRule>();
        //services.AddTransient<IRule, BalancedParticipationRule>();

        // Registering RuleSet
        services.AddTransient<RuleSet>();

        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Padel.Infrastructure"))); // Specify the migrations assembly
        return services;
    }
}
