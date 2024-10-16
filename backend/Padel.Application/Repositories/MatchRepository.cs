using Padel.Domain.Models;
using Padel.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Padel.Application.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        // Using a collection of MatchEntity instead of a ConcurrentDictionary
        private readonly List<MatchEntity> _matchEntities = new();

        // Create a new match
        public Task<bool> CreateAsync(Match match, CancellationToken token = default)
        {
            // Check if a MatchEntity with the same Id already exists
            if (_matchEntities.Any(m => m.Id == match.Id))
            {
                return Task.FromResult(false); // Match already exists
            }

            // Create a MatchEntity from the Match model
            var entity = new MatchEntity
            {
                Id = match.Id,
                SeasonId = match.SeasonId,
                Date = match.MatchDate,
                Team1Id = match.Team1Id,
                Team2Id = match.Team2Id
            };

            //// Check if the season exists (in a real scenario, this check would be against a seasons collection)
            //if (!_matchEntities.Any(m => m.SeasonId == match.SeasonId))
            //{
            //    return Task.FromResult(false); // Season does not exist
            //}

            _matchEntities.Add(entity); // Add the new MatchEntity
            return Task.FromResult(true); // Return success
        }

        // Delete a match by its Id
        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            var removedCount = _matchEntities.RemoveAll(m => m.Id == id);
            return Task.FromResult(removedCount > 0); // Returns true if a match was deleted
        }

        // Check if a match exists by its Id
        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            return Task.FromResult(_matchEntities.Any(m => m.Id == id)); // Returns true if the match exists
        }

        // Get all matches
        public Task<IEnumerable<Match>> GetAllAsync(CancellationToken token = default)
        {
            var result = _matchEntities.Select(entity => new Match
            {
                Id = entity.Id,
                SeasonId = entity.SeasonId,
                MatchDate = entity.Date,
                Team1Id = entity.Team1Id,
                Team2Id = entity.Team2Id,
            }).ToList();

            return Task.FromResult<IEnumerable<Match>>(result);
        }

        // Get a match by its Id
        public Task<Match?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var matchEntity = _matchEntities.SingleOrDefault(m => m.Id == id);
            return Task.FromResult(matchEntity == null ? null : new Match
            {
                Id = matchEntity.Id,
                SeasonId = matchEntity.SeasonId,
                MatchDate = matchEntity.Date,
                Team1Id = matchEntity.Team1Id,
                Team2Id = matchEntity.Team2Id
            });
        }

        // Update an existing match
        public Task<bool> UpdateAsync(Match match, CancellationToken token = default)
        {
            // Find the index of the MatchEntity to update
            var matchIndex = _matchEntities.FindIndex(m => m.Id == match.Id);
            if (matchIndex == -1) return Task.FromResult(false); // Match not found

            // Update the existing MatchEntity
            var existingMatchEntity = _matchEntities[matchIndex];
            existingMatchEntity.SeasonId = match.SeasonId; // Assuming SeasonId can be updated
            existingMatchEntity.Date = match.MatchDate; // Update the match date

            return Task.FromResult(true); // Return success
        }

        // Get matches by SeasonId
        public Task<IEnumerable<Match>> GetBySeasonIdAsync(Guid seasonId, CancellationToken token = default)
        {
            var result = _matchEntities
                .Where(m => m.SeasonId == seasonId)
                .Select(entity => new Match
                {
                    Id = entity.Id,
                    SeasonId = entity.SeasonId,
                    MatchDate = entity.Date,
                    Team1Id = entity.Team1Id,
                    Team2Id = entity.Team2Id
                }).ToList();

            return Task.FromResult<IEnumerable<Match>>(result);
        }

        // Bulk create matches
        public Task<bool> CreateManyAsync(IEnumerable<Match> matches, CancellationToken token = default)
        {
            var addedMatches = 0;

            foreach (var match in matches)
            {
                Console.WriteLine($"Processing Match: Id={match.Id}, SeasonId={match.SeasonId}, MatchDate={match.MatchDate}");

                // Check if a MatchEntity with the same Id already exists
                if (_matchEntities.Any(m => m.Id == match.Id))
                {
                    Console.WriteLine($"Match with Id {match.Id} already exists, skipping.");
                    continue; // Match already exists
                }

                // Create a MatchEntity from the Match model
                var entity = new MatchEntity
                {
                    Id = match.Id,
                    SeasonId = match.SeasonId,
                    Date = match.MatchDate, 
                    Team1Id = match.Team1Id,
                    Team2Id = match.Team2Id
                };

                // Check if the season exists (this check should be improved)
                // Example: Check against a separate seasons repository
                // if (!_seasonRepository.ExistsById(match.SeasonId)) 
                // {
                //     Console.WriteLine($"Season with Id {match.SeasonId} does not exist, skipping match.");
                //     continue; // Skip this match if the season does not exist
                // }

                // For now, let's skip this condition for testing
                _matchEntities.Add(entity); // Add the new MatchEntity
                addedMatches++;

                Console.WriteLine($"Added Match: Id={entity.Id}, SeasonId={entity.SeasonId}, Date={entity.Date}");
            }

            Console.WriteLine($"Total Matches Attempted: {matches.Count()}, Total Matches Added: {addedMatches}");
            return Task.FromResult(addedMatches > 0); // Return true if at least one match was inserted
        }
    }
}
