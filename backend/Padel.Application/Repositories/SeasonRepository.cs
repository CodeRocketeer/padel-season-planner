using Dapper;
using Padel.Application.Database;

using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;


namespace Padel.Application.Repositories;

public class SeasonRepository : ISeasonRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IMatchRepository _matchRepository;

    public SeasonRepository(IDbConnectionFactory dbConnectionFactory, IMatchRepository matchRepository)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _matchRepository = matchRepository;
    }

    public async Task<bool> CreateAsync(Season Season, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();


        var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into Seasons (id, amountOfMatches, startDate, title, dayOfWeek) 
            values (@Id, @AmountOfMatches, @StartDate, @Title, @DayOfWeek)
            """, Season, cancellationToken: token));


        transaction.Commit();

        return result > 0;
    }

    public async Task<Season?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var season = await connection.QuerySingleOrDefaultAsync<Season>(
            new CommandDefinition("""
            select * from Seasons where id = @id
            """, new { id }, cancellationToken: token));

        if (season is null)
        {
            return null;
        }

        // Todo: check in the participants table if the user is already registered
        var isUserParticipating = await connection.ExecuteScalarAsync<bool>(
       new CommandDefinition("""
        select count(1) from Participants where SeasonId = @id and UserId = @userid
        """, new { id, userid }, cancellationToken: token));

        season.UserParticipates = isUserParticipating;

        // Fetch matches using the MatchRepository
        var matches = await _matchRepository.GetAllAsync(new GetAllMatchesOptions { SeasonId = id }, token);
        season.Matches = matches.ToList(); // Assuming the Matches property exists in your Season model


        return season;
    }

    public async Task<Season?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var Season = await connection.QuerySingleOrDefaultAsync<Season>(
            new CommandDefinition("""
            select * from Seasons where slug = @slug
            """, new { slug }, cancellationToken: token));

        if (Season is null)
        {
            return null;
        }

        return Season;
    }

    public async Task<IEnumerable<Season>> GetAllAsync(Guid? userid = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync<dynamic>(new CommandDefinition("""
        SELECT 
            m.id AS id,
            m.title AS title,
            m.startdate AS startdate,
            m.amountofmatches AS amountofmatches,
            m.dayofweek AS dayofweek
        FROM seasons m
        """, cancellationToken: token));

        return result.Select(x => new Season
        {
            Id = x.id,
            Title = x.title,
            StartDate = x.startdate,
            AmountOfMatches = x.amountofmatches,
            DayOfWeek = x.dayofweek

        });
    }


    public async Task<bool> UpdateAsync(Season Season, Guid? userid = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition("""
           delete from Seasons where id = @Id
           """, new { Season.Id }, cancellationToken: token));



        var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into Seasons (id, amountOfMatches, startDate, title, dayOfWeek)
            values (@Id, @AmountOfMatches, @StartDate, @Title, @DayOfWeek)
            """, Season, cancellationToken: token));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete from Seasons where id = @id
            """, new { id }, cancellationToken: token));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            select count(1) from Seasons where id = @id
            """, new { id }, cancellationToken: token));
    }
}
