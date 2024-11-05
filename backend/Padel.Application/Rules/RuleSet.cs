using Padel.Domain.Models;
using System.Collections.Generic;

namespace Padel.Application.Rules
{
    public class RuleSet
    {
        public List<IRule> Rules { get; }

        public RuleSet(IEnumerable<IRule> rules)
        {
            Rules = new List<IRule>(rules);
        }

        public int Validate(List<Match> matchSet, List<Player> playerList)
        {
            int totalFaultPoints = 0;

            foreach (var rule in Rules)
            {
                // Calculate the fault points for the current rule
                int ruleFaultPoints = rule.Validate(matchSet, playerList);

                // Factor in the weight of the rule
                int weightedFaultPoints = ruleFaultPoints * rule.Weight;

                // Add the weighted fault points to the total
                totalFaultPoints += weightedFaultPoints;

                // Log each rule's fault contribution for debugging
                System.Console.WriteLine($"{rule.GetType().Name} Fault Points (Weighted): {weightedFaultPoints}");
            }

            return totalFaultPoints; // Return total fault points for the match set
        }
    }
}
