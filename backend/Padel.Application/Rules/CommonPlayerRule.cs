using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Rules
{
    public class CommonPlayerRule : IRule
    {
        public int Weight => 100;

        public decimal Validate(Match match, List<Match> scheduledMatches, List<Participant> participants)
        {
            var team1Participants = match.Team1.GetParticipants();
            var team2Participants = match.Team2.GetParticipants();

            // Check if there are common participants between the two teams
            if (team1Participants.Any(p1 => team2Participants.Any(p2 => p1.Id == p2.Id)))
            {
                return 100; // 100% fault if any participant is on both teams
            }

            return 0; // No faults found
        }
    }
}
