using Padel.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Padel.Application.Rules
{
    public class AllPlayersParticipateRule : IRule
    {
        public int Weight => 100; // Weight indicating the importance of this rule

        public int Validate(List<Match> matchSet, List<Player> playerList)
        {
            // Return 100 fault points if there are no matches
            if (matchSet == null || !matchSet.Any())
            {
                return 100; // Complete fault if no matches
            }

            // Get a distinct list of all participants in the match set
            var allParticipants = matchSet
                .SelectMany(match => match.Team1.GetParticipants().Concat(match.Team2.GetParticipants()))
                .Distinct()
                .ToList();

            // Check if there are any players not participating
            var nonParticipatingPlayers = playerList.Except(allParticipants).ToList();

            // If any players are missing, return a fault of 100%
            if (nonParticipatingPlayers.Any())
            {
                return 100; // Complete fault if even one player is missing
            }

            // If all players are participating, return 0
            return 0; // No fault
        }
    }
}
