using Dapper;
using System;
using System.Threading.Tasks;

namespace Padel.Application.Database
{
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

            // Create Seasons Table
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Seasons (
                    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                    name VARCHAR(255) NOT NULL,
                    start_date DATE NOT NULL,
                    day_of_week INT NOT NULL CHECK (day_of_week >= 0 AND day_of_week <= 6),
                    amount_of_matches INT NOT NULL
                );
            ");

            // Create Players Table
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Players (
                    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                    user_id UUID NOT NULL,
                    name VARCHAR(255) NOT NULL,
                    sex VARCHAR(50) NOT NULL
                );
            ");

            // Create Teams Table
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Teams (
                    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                    season_id UUID NOT NULL,
                    FOREIGN KEY (season_id) REFERENCES Seasons(id) ON DELETE CASCADE
                );
            ");

            // Create Team-Player Link Table (Many-to-Many relationship between Teams and Players)
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS TeamPlayers (
                    team_id UUID NOT NULL,
                    player_id UUID NOT NULL,
                    PRIMARY KEY (team_id, player_id),
                    FOREIGN KEY (team_id) REFERENCES Teams(id) ON DELETE CASCADE,
                    FOREIGN KEY (player_id) REFERENCES Players(id) ON DELETE CASCADE
                );
            ");

            // Create Matches Table
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Matches (
                    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                    season_id UUID NOT NULL,
                    match_date DATE NOT NULL,
                    FOREIGN KEY (season_id) REFERENCES Seasons(id) ON DELETE CASCADE
                );
            ");

            // Create Match-Team Link Table (Many-to-Many relationship between Matches and Teams)
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS MatchTeams (
                    match_id UUID NOT NULL,
                    team_id UUID NOT NULL,
                    PRIMARY KEY (match_id, team_id),
                    FOREIGN KEY (match_id) REFERENCES Matches(id) ON DELETE CASCADE,
                    FOREIGN KEY (team_id) REFERENCES Teams(id) ON DELETE CASCADE
                );
            ");
        }
    }
}
