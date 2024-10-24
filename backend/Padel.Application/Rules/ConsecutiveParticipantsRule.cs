using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Rules
{
    public class ConsecutiveParticipantsRule : IRule
    {
        public int Weight => 80; // Importance of this rule

        public decimal Validate(Match match, List<Match> scheduledMatches, List<Participant> participants)
        {
            // Get last match from the planned matches
            var lastMatch = scheduledMatches
                .OrderByDescending(m => m.MatchDate)
                .FirstOrDefault();

            // If there's no last match, return 0 (no fault)
            if (lastMatch == null)
                return 0;

            // Get participants from both teams in the last match
            var lastMatchParticipants = lastMatch.Team1.GetParticipants()
                .Concat(lastMatch.Team2.GetParticipants())
                .ToList();

            // Get participants from both teams in the current match
            var currentMatchParticipants = match.Team1.GetParticipants()
                .Concat(match.Team2.GetParticipants())
                .ToList();

            // Check if any participants from the current match played in the last match
            if (currentMatchParticipants.Any(p => lastMatchParticipants.Contains(p)))
            {
                return 100; // Return full fault if participants played consecutive matches
            }

            return 0;
        }   
    }
}
