using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Padel.Domain.Models;
using Padel.Application.Repositories;
using FluentValidation;
using Padel.Application.Validators;

namespace Padel.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IValidator<Match> _matchValidator;

        public MatchService(IMatchRepository matchRepository,ISeasonRepository seasonRepository, IValidator<Match> matchValidator)
        {
            _matchValidator = matchValidator;
            _matchRepository = matchRepository;
            _seasonRepository = seasonRepository;
        }

        public async Task<bool> CreateAsync(Match match, CancellationToken token = default)
        {
            // Match validation occurs in the Match model
            await _matchValidator.ValidateAndThrowAsync(match, token);
            // Check if the specified season exists
            var seasonExists = await _seasonRepository.ExistsByIdAsync(match.SeasonId, token );
            if (!seasonExists)
            {
                throw new InvalidOperationException("Cannot create match. The specified season does not exist.");
            }
            // Create the match
            return await _matchRepository.CreateAsync(match, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _matchRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Match>> GetAllAsync(CancellationToken token = default)
        {
            return _matchRepository.GetAllAsync( token);
        }

        public Task<Match?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _matchRepository.GetByIdAsync(id, token);
        }

        public async Task<Match?> UpdateAsync(Match match, CancellationToken token = default)
        {
            // Match validation occurs in the Match model
            await _matchValidator.ValidateAndThrowAsync(match, token);
            // Check if the specified match exists
            var matchExists = await _matchRepository.ExistsByIdAsync(match.Id, token);
            if (!matchExists) return null;

            // Update the match
            await _matchRepository.UpdateAsync(match, token);
            return match;
        }

        public async Task<IEnumerable<Match>> GetAllBySeasonIdAsync(Guid seasonId, CancellationToken token = default)
        {
            return await _matchRepository.GetBySeasonIdAsync(seasonId,token);
        }
    }
}
