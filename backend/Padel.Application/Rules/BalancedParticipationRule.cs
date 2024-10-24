using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Padel.Application.Rules
{
    public class BalancedParticipationRule : IRule
    {
        public int Weight => 85; // Importance of this rule

        public decimal Validate(Match match, List<Match> scheduledMatches, List<Participant> totalParticipants)
        {
            // Step 1: Check for edge cases: No matches or no participants
            if (scheduledMatches == null || !scheduledMatches.Any() || totalParticipants == null || !totalParticipants.Any())
            {
                return 0; // No imbalance if there are no scheduled matches or participants.
            }

            // Step 2: Gather participants from both teams in the current match
            var currentMatchParticipants = match.Team1.GetParticipants()
                .Concat(match.Team2.GetParticipants())
                .ToList();

            // Step 3: Track how many matches each participant has played so far
            var participationCounts = scheduledMatches
                .SelectMany(m => m.Team1.GetParticipants().Concat(m.Team2.GetParticipants()))
                .GroupBy(p => p.Id)
                .ToDictionary(g => g.Key, g => g.Count());

            // Step 4: Identify participants who have never played
            var neverPlayedParticipants = totalParticipants
                .Where(p => !participationCounts.ContainsKey(p.Id))
                .ToList();

            // Step 5: Prioritize players who have never played
            if (neverPlayedParticipants.Any(p => currentMatchParticipants.Any(cp => cp.Id == p.Id)))
            {
                return 0; // No fault: match includes participants who haven't played yet.
            }

            // Step 6: Calculate the minimum play count (least played participants)
            var minPlayCount = participationCounts.Values.DefaultIfEmpty(0).Min();

            // Step 7: Identify participants who have played the least
            var leastPlayedParticipants = totalParticipants
                .Where(p => participationCounts.TryGetValue(p.Id, out var count) && count == minPlayCount)
                .ToList();

            // Step 8: Check if all participants in the current match are among the least played
            var leastPlayedInMatch = currentMatchParticipants
                .Where(p => leastPlayedParticipants.Any(lp => lp.Id == p.Id))
                .ToList();

            // Step 9: Apply the rule
            if (leastPlayedInMatch.Count == currentMatchParticipants.Count)
            {
                return 0; // No fault: all participants in the match are among the least played.
            }

            // Step 10: Return full fault if the match is imbalanced
            return 100; // Full fault: not enough least played participants in this match.
        }
    }
}
