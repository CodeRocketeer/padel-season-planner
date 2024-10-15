using Dapper;
using Padel.Application.Database;
using Padel.Domain.Models;

namespace Padel.Application.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MatchRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        // Create a new match
        public async Task<bool> CreateAsync(Match match, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            // Check if the season exists
            var seasonExists = await connection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Seasons WHERE id = @SeasonId",
                new { SeasonId = match.SeasonId },
                transaction
            );

            if (!seasonExists)
            {
                // If the season does not exist, return false and don't create the match
                transaction.Rollback();
                return false;
            }

            // Create the match if the season exists
            var entity = new
            {
                Id = match.Id,
                SeasonId = match.SeasonId,
                MatchDate = match.MatchDate
            };

            var result = await connection.ExecuteAsync(new CommandDefinition(@"
                INSERT INTO Matches (id, season_id, match_date) 
                VALUES (@Id, @SeasonId, @MatchDate)",
                entity,
                transaction
            ));

            transaction.Commit();
            return result > 0;
        }

        // Delete a match by its Id
        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                "DELETE FROM Matches WHERE id = @Id;",
                new { Id = id }
            );

            return result > 0; // Returns true if a row was deleted
        }

        // Check if a match exists by its Id
        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Matches WHERE id = @Id;",
                new { Id = id }
            );

            return count > 0; // Returns true if the match exists
        }

        // Get all matches
        public async Task<IEnumerable<Match>> GetAllAsync(CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var result = await connection.QueryAsync<Match>(
                "SELECT id, season_id as SeasonId, match_date as MatchDate FROM Matches"
            );

            return result.ToList();
        }

        // Get a match by its Id
        public async Task<Match?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var match = await connection.QuerySingleOrDefaultAsync<Match>(
                "SELECT id, season_id as SeasonId, match_date as MatchDate FROM Matches WHERE id = @Id",
                new { Id = id }
            );

            return match;
        }

        // Update an existing match
        public async Task<bool> UpdateAsync(Match match, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            // Check if the season exists
            var seasonExists = await connection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Seasons WHERE id = @SeasonId",
                new { SeasonId = match.SeasonId }
            );

            if (!seasonExists)
            {
                // If the season does not exist, return false and don't update the match
                return false;
            }

            var entity = new
            {
                Id = match.Id,
                SeasonId = match.SeasonId,
                MatchDate = match.MatchDate
            };

            var result = await connection.ExecuteAsync(@"
                UPDATE Matches 
                SET season_id = @SeasonId, match_date = @MatchDate 
                WHERE id = @Id;",
                entity
            );

            return result > 0; // Returns true if the update was successful
        }


        // Get matches by SeasonId
        public async Task<IEnumerable<Match>> GetBySeasonIdAsync(Guid seasonId, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var matches = await connection.QueryAsync<Match>(
                "SELECT id, season_id as SeasonId, match_date as MatchDate FROM Matches WHERE season_id = @SeasonId",
                new { SeasonId = seasonId }
            );

            return matches.ToList();
        }

        public async Task<bool> CreateMatchesAsync(IEnumerable<Match> matches, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
            

                // Prepare entities for bulk insert
                var matchEntities = matches.Select(match => new
                {
                    Id = match.Id,
                    SeasonId = match.SeasonId,
                    MatchDate = match.MatchDate
                }).ToList();

                // Bulk insert
                var result = await connection.ExecuteAsync(@"
            INSERT INTO Matches (id, season_id, match_date) 
            VALUES (@Id, @SeasonId, @MatchDate);",
                    matchEntities, transaction);

                transaction.Commit();
                return result == matches.Count(); // Return true if all matches were inserted
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw; // Rethrow the exception after rollback
            }
        }
    }
}
