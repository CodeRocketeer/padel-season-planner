using Padel.Application.Models;


namespace Padel.Application.Repositories.Interfaces;

public interface IParticipantRepository
{
    Task<bool> ParticipateInSeasonAsync(Player player, CancellationToken token = default);

    Task<bool> ExistsBySeasonAndUserIdAsync(Guid seasonId, Guid userId, CancellationToken token = default);
    Task<bool> LeaveSeasonAsync(Guid seasonId, Guid userId, CancellationToken token = default);

    Task<IEnumerable<Player>> GetAllAsync(GetAllParticipantsOptions options, CancellationToken token = default);

    Task<bool> CreateManyAsync(List<Player> participants, CancellationToken token = default);
}
