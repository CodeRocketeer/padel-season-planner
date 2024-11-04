using Padel.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Padel.Application.Rules
{
    public class BalancedParticipationRule : IRule
    {
        public int Weight => 100; // Base weight for this rule

        public int Validate(List<Match> matchSet, List<Player> playerList)
        {
            if (matchSet == null || !matchSet.Any())
            {
                return 0; // No matches to validate
            }

            var playerParticipation = new Dictionary<Player, int>();
            foreach (var match in matchSet)
            {
                var participants = match.Team1.GetParticipants().Concat(match.Team2.GetParticipants());
                foreach (var player in participants)
                {
                    if (!playerParticipation.ContainsKey(player))
                    {
                        playerParticipation[player] = 0;
                    }
                    playerParticipation[player]++;
                }
            }

            int totalMatches = matchSet.Count;
            int expectedParticipation = playerParticipation.Count > 0 ? totalMatches * 2 / playerParticipation.Count : 0;
            int faultPoints = 0;

            foreach (var count in playerParticipation.Values)
            {
                if (count > expectedParticipation + 1)
                {
                    faultPoints += count - (expectedParticipation + 1); // Count excess participations as faults
                }
            }

            return faultPoints; // Return total fault points
        }
    }
}
