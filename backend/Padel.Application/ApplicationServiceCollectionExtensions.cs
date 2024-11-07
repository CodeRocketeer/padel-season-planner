using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Padel.Infrastructure;
using Padel.Application.Rules;
using Padel.Application.Services;
using Padel.Application.Services.Interfaces;
using Padel.Infrastructure.Repositories.Interfaces;
using Padel.Infrastructure.Repositories;
using Padel.Application.Generators;


namespace Padel.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISeasonRepository, SeasonRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();    
        services.AddScoped<ISeasonService, SeasonService>();
        services.AddScoped<IPlayerService, PlayerService>();
        //services.AddScoped<ISeederService, SeederService>();

        services.AddScoped<SeasonScheduleGenerator>();
        services.AddScoped<TeamGenerator>();
        services.AddScoped<MatchGenerator>();


        // Registering rules as transient (or singleton, depending on your needs)
        services.AddScoped<IRule, ConsecutiveParticipantsRule>();
        services.AddScoped<IRule, AllPlayersParticipateRule>();
        services.AddScoped<IRule, GenderBalanceRule>();
        services.AddScoped<IRule, BalancedParticipationRule>();

        // Registering RuleSet
        services.AddScoped<RuleSet>();

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
