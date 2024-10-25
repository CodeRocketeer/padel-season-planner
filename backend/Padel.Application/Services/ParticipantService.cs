using FluentValidation;

using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;
using Padel.Application.Services.Interfaces;

namespace Padel.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IValidator<Player> _participantValidator;
        private readonly IValidator<GetAllParticipantsOptions> _optionsValidator;
        private readonly ISeasonRepository _seasonRepository;

        public ParticipantService(IParticipantRepository participantRepository, IValidator<Player> participantValidator, ISeasonRepository seasonRepository, IValidator<GetAllParticipantsOptions> optionsValidator)
        {
            _participantRepository = participantRepository;
            _participantValidator = participantValidator;
            _seasonRepository = seasonRepository;
            _optionsValidator = optionsValidator;

        }

        public async Task<bool> ParticipateInSeasonAsync(Player player, CancellationToken token)
        {
            await _participantValidator.ValidateAndThrowAsync(player, cancellationToken: token);
            var seasonExists = await _seasonRepository.ExistsByIdAsync(player.SeasonId, token);
            if (!seasonExists)
                throw new DirectoryNotFoundException(message: $"Season with ID '{player.SeasonId}' does not exist.");
            var ParticipantExists = await _participantRepository.ExistsBySeasonAndUserIdAsync(player.SeasonId, player.UserId, token);
            if (ParticipantExists)
                throw new DirectoryNotFoundException(message: $"User with ID '{player.UserId}' is already a player in the season with ID '{player.SeasonId}'.");

            return await _participantRepository.ParticipateInSeasonAsync(player, token);
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

        public async Task<IEnumerable<Player>> GetAllAsync(GetAllParticipantsOptions options, CancellationToken token = default)
        {

            await _optionsValidator.ValidateAndThrowAsync(options, token);
            return await _participantRepository.GetAllAsync(options, token);
        }
    }
}
