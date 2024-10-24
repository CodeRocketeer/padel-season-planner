using Dapper;
using Padel.Application.Database;
using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;


namespace Padel.Application.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ParticipantRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> ParticipateInSeasonAsync(Participant participant, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into Participants (id, userId, seasonId, gender, name) 
            values (@Id, @UserId, @SeasonId, @Gender, @Name)
            """, participant, cancellationToken: token));


        transaction.Commit();

        return result > 0;
    }


    public async Task<bool> ExistsBySeasonAndUserIdAsync(Guid seasonId, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            select count(1) from Participants where seasonId = @seasonId and userId = @userId
            """, new { seasonId, userId }, cancellationToken: token));
    }

    public async Task<bool> LeaveSeasonAsync(Guid seasonId, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            delete from Participants where seasonId = @seasonId and userId = @userId
            """, new { seasonId, userId }, cancellationToken: token));

        transaction.Commit();
        return result > 0;
    }

    public async Task<IEnumerable<Participant>> GetAllAsync(GetAllParticipantsOptions options, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync<Participant>(new CommandDefinition("""
            select * from Participants where (@seasonId is null or seasonId = @seasonId) and (@userId is null or userId = @userId)
            """, new
        {
            seasonId = options.SeasonId,
            userId = options.UserId
        }, cancellationToken: token));

        return result.Select(x => new Participant
        {
            Id = x.Id,
            UserId = x.UserId,
            SeasonId = x.SeasonId,
            Gender = x.Gender,
            Name = x.Name

        });

    }


     public async Task<bool> CreateManyAsync(List<Participant> participants, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();


        foreach (var participant in participants)
        {
            var result = await connection.ExecuteAsync(new CommandDefinition("""
            insert into Participants (id, userId, seasonId, gender, name) 
            values (@Id, @UserId, @SeasonId, @Gender, @Name)
            """, participant, cancellationToken: token));

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
