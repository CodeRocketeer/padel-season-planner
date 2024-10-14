using Dapper;
using Padel.Application.Database;
using Padel.Application.Repositories;
using Padel.Domain.Models;
using System.Data;

public class SeasonRepository : ISeasonRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SeasonRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Season season, CancellationToken token = default)
    {
        if (season.AmountOfMatches <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(season.AmountOfMatches), "Amount of matches must be greater than zero.");
        }

        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var entity = new
        {
            Id = season.Id,
            Name = season.Name,
            StartDate = season.StartDate,
            DayOfWeek = season.DayOfWeek,
            AmountOfMatches = season.AmountOfMatches
        };

        var result = await connection.ExecuteAsync(new CommandDefinition(@"
            INSERT INTO Seasons (id, name, start_date, day_of_week, amount_of_matches) 
            VALUES (@Id, @Name, @StartDate, @DayOfWeek, @AmountOfMatches)", entity, transaction));

        transaction.Commit();
        return result > 0; // Returns true if the insert was successful
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var command = "DELETE FROM Seasons WHERE id = @Id;";
        var result = await connection.ExecuteAsync(command, new { Id = id });
        return result > 0; // Returns true if a row was deleted
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var command = "SELECT COUNT(1) FROM Seasons WHERE id = @Id;";
        var count = await connection.ExecuteScalarAsync<int>(command, new { Id = id });
        return count > 0; // Returns true if a season exists
    }

    public async Task<IEnumerable<Season>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync(new CommandDefinition(@"SELECT * FROM Seasons"));

        return result.Select(x => new Season
        {
            Id = x.id, // Ensure property names match the database query result
            Name = x.name,
            StartDate = x.start_date,
            DayOfWeek = x.day_of_week,
            AmountOfMatches = x.amount_of_matches
        });
    }

    public async Task<Season?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        // Query for the season
        var seasonQuery = "SELECT * FROM Seasons WHERE id = @Id;";

        // Query for the matches belonging to the season
        var matchesQuery = "SELECT id, season_id as SeasonId, match_date as MatchDate FROM Matches WHERE season_id = @SeasonId;";

        // Execute the season query
        var entity = await connection.QuerySingleOrDefaultAsync<dynamic>(seasonQuery, new { Id = id });
        if (entity == null) return null;

        // Execute the matches query
        var matches = await connection.QueryAsync<Match>(matchesQuery, new { SeasonId = id });

        // Return the season with the matches
        return new Season
        {
            Id = entity.id,
            Name = entity.name,
            StartDate = entity.start_date,
            DayOfWeek = entity.day_of_week,
            AmountOfMatches = entity.amount_of_matches,
            Matches = matches.ToList() // Assign the matches to the season
        };
    }


    public async Task<bool> UpdateAsync(Season season, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var command = @"
            UPDATE Seasons 
            SET name = @Name, start_date = @StartDate, day_of_week = @DayOfWeek, amount_of_matches = @AmountOfMatches 
            WHERE id = @Id;";

        var entity = new
        {
            Id = season.Id,
            Name = season.Name,
            StartDate = season.StartDate,
            DayOfWeek = season.DayOfWeek,
            AmountOfMatches = season.AmountOfMatches
        };

        var result = await connection.ExecuteAsync(command, entity);
        return result > 0; // Returns true if the update was successful
    }
}
