using Padel.Domain.Models;

namespace Padel.Application.Rules
{
    public class ConsecutiveParticipantsRule : IRule
    {
        public int Weight => 80; // Importance of this rule

        public decimal Validate(Match match, List<Match> scheduledMatches)
        {
            if (scheduledMatches == null || !scheduledMatches.Any())
            {
                return 0; // No previous match, so no fault
            }

            var lastAddedMatch = scheduledMatches.Last();

            var lastAddedMatchPlayers = lastAddedMatch.Team1.GetParticipants().Concat(lastAddedMatch.Team2.GetParticipants()).ToList();
            var currentMatchPlayers = match.Team1.GetParticipants().Concat(match.Team2.GetParticipants()).ToList();

            if (currentMatchPlayers.Any(player => lastAddedMatchPlayers.Contains(player)))
            {
                return 100; 
            }

            return 0;
        }
    }
}
