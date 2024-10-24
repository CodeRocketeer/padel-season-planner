using Padel.Application.Models;

namespace Padel.Application.Rules
{
    public class RuleSet
    {
        public List<IRule> Rules { get; }

        // Constructor accepting the rules as a dependency
        public RuleSet(IEnumerable<IRule> rules)
        {
            Rules = new List<IRule>(rules);
        }

        public decimal Validate(Match match, List<Match> scheduledMatches, List<Participant> participants)
        {
            decimal totalFaultPercentage = 0;

            foreach (var rule in Rules)
            {
                totalFaultPercentage += rule.Validate(match, scheduledMatches, participants) * (rule.Weight / 100m); // Weighted fault percentage
            }

            return totalFaultPercentage; // Total fault percentage for the matches
        }
    }
}