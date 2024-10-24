using Padel.Application.Models;


namespace Padel.Application.Repositories.Interfaces;

public interface IParticipantRepository
{
    Task<bool> ParticipateInSeasonAsync(Participant participant, CancellationToken token = default);

    Task<bool> ExistsBySeasonAndUserIdAsync(Guid seasonId, Guid userId, CancellationToken token = default);
    Task<bool> LeaveSeasonAsync(Guid seasonId, Guid userId, CancellationToken token = default);

    Task<IEnumerable<Participant>> GetAllAsync(GetAllParticipantsOptions options, CancellationToken token = default);

    Task<bool> CreateManyAsync(List<Participant> participants, CancellationToken token = default);
}
