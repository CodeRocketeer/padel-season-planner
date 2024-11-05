using Padel.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Padel.Application.Rules
{
    public class ConsecutiveParticipantsRule : IRule
    {
        public int Weight => 50; // Base weight for this rule

        public int Validate(List<Match> matchSet, List<Player> playerList)
        {
            if (matchSet == null || !matchSet.Any())
            {
                return 100; // Max fault if there are no matches
            }

            int violationCount = 0;
            var consecutiveMatches = new Dictionary<Player, int>();

            for (int i = 0; i < matchSet.Count; i++)
            {
                var currentMatch = matchSet[i];
                var currentPlayers = currentMatch.Team1.GetParticipants().Concat(currentMatch.Team2.GetParticipants()).ToList();

                foreach (var player in currentPlayers)
                {
                    // If the player was already counted in the previous match
                    if (i > 0 && consecutiveMatches.ContainsKey(player) && consecutiveMatches[player] > 0)
                    {
                        // Increment the count of consecutive matches for this player
                        consecutiveMatches[player]++;
                    }
                    else
                    {
                        // Initialize or reset the count
                        consecutiveMatches[player] = 1;
                    }

                    // Count as a fault if they participated in more than 1 consecutive match
                    if (consecutiveMatches[player] > 1)
                    {
                        violationCount++; // Increment violation count for each consecutive match
                    }
                }

                // Reset the count for players who did not participate in this match
                foreach (var player in consecutiveMatches.Keys.ToList())
                {
                    if (!currentPlayers.Contains(player))
                    {
                        consecutiveMatches[player] = 0; // Reset if not participating
                    }
                }
            }

            // Return the total count of violations as fault points
            return violationCount;
        }
    }
}
