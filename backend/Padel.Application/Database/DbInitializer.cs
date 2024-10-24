using Dapper;

namespace Padel.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        // Create Seasons table
        await connection.ExecuteAsync("""
                CREATE TABLE IF NOT EXISTS seasons (
                    id UUID PRIMARY KEY,
                    amountOfMatches INTEGER NOT NULL,
                    startDate DATE NOT NULL,
                    title TEXT NOT NULL,
                    dayOfWeek INTEGER NOT NULL
                );
            """);

        // Create participants table
        await connection.ExecuteAsync("""
                CREATE TABLE IF NOT EXISTS participants (
                    id UUID PRIMARY KEY,
                    userId UUID NOT NULL,
                    seasonId UUID REFERENCES seasons(id) ON DELETE CASCADE,
                    gender TEXT NOT NULL,
                    name TEXT NOT NULL
                );
            """);

        // Create Teams table with participant1Id and participant2Id
        await connection.ExecuteAsync("""
                CREATE TABLE IF NOT EXISTS teams (
                    id UUID PRIMARY KEY,
                    seasonId UUID REFERENCES seasons(id) ON DELETE CASCADE,
                    participant1Id UUID REFERENCES participants(id) ON DELETE CASCADE,
                    participant2Id UUID REFERENCES participants(id) ON DELETE CASCADE
                );
            """);

        // Create Matches table
        await connection.ExecuteAsync("""
                CREATE TABLE IF NOT EXISTS matches (
                    id UUID PRIMARY KEY,
                    seasonId UUID REFERENCES seasons(id) ON DELETE CASCADE,
                    team1Id UUID REFERENCES teams(id) ON DELETE CASCADE,
                    team2Id UUID REFERENCES teams(id) ON DELETE CASCADE,
                    matchDate DATE NOT NULL
                );
            """);
    }
}
