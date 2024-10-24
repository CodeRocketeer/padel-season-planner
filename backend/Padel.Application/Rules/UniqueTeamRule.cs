using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Rules
{
    public class UniqueTeamRule : IRule
    {
        public int Weight => 50; // Importance of this rule

        public decimal Validate(Match match , List<Match> scheduledMatches, List<Participant> participants)
        {
            // Check if the team IDs are the same
            if (match.Team1.Id == match.Team2.Id)
            {
                return 100; // 100% fault if team IDs are the same
            }

            return 0; // Return 0 if no faults found
        }
    }
}
