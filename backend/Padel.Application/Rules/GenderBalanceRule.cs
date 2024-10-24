using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Rules
{
    public class GenderBalanceRule : IRule
    {
        public int Weight => 90;

        public decimal Validate(Match match, List<Match> scheduledMatches, List<Participant> participants)
        {
            var team1Participants = match.Team1.GetParticipants();
            var team2Participants = match.Team2.GetParticipants();

            // Count male and female participants
            int team1MaleCount = team1Participants.Count(p => p.Gender == "M");
            int team1FemaleCount = team1Participants.Count(p => p.Gender == "F");
            int team2MaleCount = team2Participants.Count(p => p.Gender == "M");
            int team2FemaleCount = team2Participants.Count(p => p.Gender == "F");

            // Condition 1: MM vs MM (both teams have 2 males)
            bool isMMvsMM = (team1MaleCount == 2 && team1FemaleCount == 0) &&
                            (team2MaleCount == 2 && team2FemaleCount == 0);

            // Condition 2: FF vs FF (both teams have 2 females)
            bool isFFvsFF = (team1MaleCount == 0 && team1FemaleCount == 2) &&
                            (team2MaleCount == 0 && team2FemaleCount == 2);

            // Condition 3: FM vs FM (both teams have 1 male and 1 female)
            bool isFMvsFM = (team1MaleCount == 1 && team1FemaleCount == 1) &&
                            (team2MaleCount == 1 && team2FemaleCount == 1);

            // Validate if any of the conditions are met
            if (isMMvsMM || isFFvsFF || isFMvsFM)
            {
                return 0; // No faults if gender balance matches one of the valid patterns
            }

            // If none of the valid gender compositions are met, return a fault
            return 100; // 100% fault for invalid gender balance
        }
    }
}
