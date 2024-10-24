
using Dapper;
using Padel.Application.Database;
using Padel.Application.Repositories.Interfaces;

namespace Padel.Application.Repositories
{
    internal class TeamRepository : ITeamRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;


        public TeamRepository(IDbConnectionFactory dbConnectionFactory )
        {
            _dbConnectionFactory = dbConnectionFactory;
         
        }

        public async Task<bool> CreateManyAsync(List<Models.Team> teams, CancellationToken token = default)
        {
            // Manual mapping from Models.Team to Database.Entities.Team
            var teamEntities = teams.Select(team => new Database.Entities.Team
            {
                Id = team.Id,
                SeasonId = team.SeasonId,
                Participant1Id = team.Participant1?.Id ?? Guid.Empty, // Handle null case
                Participant2Id = team.Participant2?.Id ?? Guid.Empty  // Handle null case
            }).ToList();

            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            foreach (var team in teamEntities)
            {
                var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into Teams (Id, SeasonId, Participant1Id, Participant2Id)
                values (@Id, @SeasonId, @Participant1Id, @Participant2Id)
                """, team, cancellationToken: token));

                if (result <= 0)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            transaction.Commit();
            return true;
        }


    }
}
