

using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;
using Padel.Application.Services.Interfaces;

namespace Padel.Application.Services;

public class SeederService : ISeederService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly ISeasonRepository _seasonRepository;


    public SeederService(IParticipantRepository participantRepository, ISeasonRepository seasonRepository)
    {
        _participantRepository = participantRepository;
        _seasonRepository = seasonRepository;

    }

    public async Task<bool> CreateManyParticipantsAsync(int count, Guid seasonId, CancellationToken token = default)
    {
        var seasonExists = await _seasonRepository.ExistsByIdAsync(seasonId, token);
        if (!seasonExists)
            throw new DirectoryNotFoundException(message: $"Season with ID '{seasonId}' does not exist.");
        // Create participants
        var participants = new List<Participant>();
      

        for (int i = 0; i < count; i++)
        {
            participants.Add(new Participant
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Gender = i % 2 == 0 ? "M" : "F",
                SeasonId = seasonId,
                Name = $"Participant {i + 1}"
            });

        }


        return await _participantRepository.CreateManyAsync(participants, token);
    }


}
