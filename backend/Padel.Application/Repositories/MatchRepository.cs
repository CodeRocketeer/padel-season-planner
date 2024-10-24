
using Dapper;
using Padel.Application.Database;
using Padel.Application.Database.Entities;
using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;

namespace Padel.Application.Repositories
{
    internal class MatchRepository : IMatchRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        

        public MatchRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            
        }

        public async Task<bool> CreateManyAsync(List<Models.Match> matches, CancellationToken token = default)
        {

            // Correct mapping to a list of entities
            var matchEntities = matches.Select(x => new Database.Entities.Match
            {
                Id = x.Id,
                SeasonId = x.SeasonId,
                Team1Id = x.Team1.Id,
                Team2Id = x.Team2.Id,
                MatchDate = x.MatchDate,

            });

            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();


            foreach (var match in matchEntities)
            {
                var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into Matches (Id, SeasonId, Team1Id, Team2Id, Matchdate)
                values (@Id, @SeasonId, @Team1Id, @Team2Id, @Matchdate)
                """, match, cancellationToken: token));

                if (result <= 0)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            transaction.Commit();

            return true;

        }

        public async Task<IEnumerable<Models.Match>> GetAllAsync(GetAllMatchesOptions options, CancellationToken token = default)
        {

            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            var result = await connection.QueryAsync<Database.Entities.Match>(new CommandDefinition("""
            select * from Matches where (@seasonId is null or seasonId = @seasonId)
            """, new
            {
                seasonId = options.SeasonId,
            }, cancellationToken: token));



            return result.Select(x => new Models.Match(x.Team1Id, x.Team2Id, x.MatchDate)
            {
                Id = x.Id,
                SeasonId = x.SeasonId
            });

        }


    }
}
