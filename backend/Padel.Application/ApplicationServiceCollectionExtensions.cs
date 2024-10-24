using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Padel.Application.Database;
using Padel.Application.Repositories;
using Padel.Application.Repositories.Interfaces;
using Padel.Application.Rules;
using Padel.Application.Services;
using Padel.Application.Services.Interfaces;

namespace Padel.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ISeasonRepository, SeasonRepository>();
        services.AddSingleton<IParticipantRepository, ParticipantRepository>();
        services.AddSingleton<ITeamRepository, TeamRepository>();
        services.AddSingleton<IMatchRepository, MatchRepository>();
        services.AddSingleton<IParticipantService, ParticipantService>();
        services.AddSingleton<ISeasonService, SeasonService>();
        services.AddSingleton<ISeederService, SeederService>();
        services.AddSingleton<ITeamService, TeamService>();
        services.AddSingleton<IMatchService, MatchService>();
       

        // Registering rules as transient (or singleton, depending on your needs)
        services.AddTransient<IRule, UniqueTeamRule>();
        services.AddTransient<IRule, GenderBalanceRule>();
        services.AddTransient<IRule, CommonPlayerRule>();
        services.AddTransient<IRule, ConsecutiveParticipantsRule>();
        services.AddTransient<IRule, BalancedParticipationRule>();

        // Registering RuleSet
        services.AddSingleton<RuleSet>();

        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => 
            new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}
