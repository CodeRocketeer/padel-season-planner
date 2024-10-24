using FluentValidation;

using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;
using Padel.Application.Services.Interfaces;

namespace Padel.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IValidator<Participant> _participantValidator;
        private readonly IValidator<GetAllParticipantsOptions> _optionsValidator;
        private readonly ISeasonRepository _seasonRepository;

        public ParticipantService(IParticipantRepository participantRepository, IValidator<Participant> participantValidator, ISeasonRepository seasonRepository, IValidator<GetAllParticipantsOptions> optionsValidator)
        {
            _participantRepository = participantRepository;
            _participantValidator = participantValidator;
            _seasonRepository = seasonRepository;
            _optionsValidator = optionsValidator;

        }

        public async Task<bool> ParticipateInSeasonAsync(Participant participant, CancellationToken token)
        {
            await _participantValidator.ValidateAndThrowAsync(participant, cancellationToken: token);
            var seasonExists = await _seasonRepository.ExistsByIdAsync(participant.SeasonId, token);
            if (!seasonExists)
                throw new DirectoryNotFoundException(message: $"Season with ID '{participant.SeasonId}' does not exist.");
            var ParticipantExists = await _participantRepository.ExistsBySeasonAndUserIdAsync(participant.SeasonId, participant.UserId, token);
            if (ParticipantExists)
                throw new DirectoryNotFoundException(message: $"User with ID '{participant.UserId}' is already a participant in the season with ID '{participant.SeasonId}'.");

            return await _participantRepository.ParticipateInSeasonAsync(participant, token);
        }

        public async Task<bool> LeaveSeasonAsync(Guid seasonId, Guid userId, CancellationToken token = default)
        {
            var seasonExists = await _seasonRepository.ExistsByIdAsync(seasonId, token);
            if (!seasonExists)
                throw new DirectoryNotFoundException(message: $"Season with ID '{seasonId}' does not exist.");
            var ParticipantExists = await _participantRepository.ExistsBySeasonAndUserIdAsync(seasonId, userId, token);
            if (!ParticipantExists)
                throw new DirectoryNotFoundException(message: $"User with ID '{userId}' is not a participant in the season with ID '{seasonId}'.");
            return await _participantRepository.LeaveSeasonAsync(seasonId, userId, token);

        }

        public async Task<IEnumerable<Participant>> GetAllAsync(GetAllParticipantsOptions options, CancellationToken token = default)
        {

            await _optionsValidator.ValidateAndThrowAsync(options, token);
            return await _participantRepository.GetAllAsync(options, token);
        }
    }
}
