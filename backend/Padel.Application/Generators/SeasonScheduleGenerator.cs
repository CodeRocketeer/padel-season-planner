using Padel.Application.Rules;
using Padel.Domain.Models;

namespace Padel.Application.Generators
{
    public class SeasonScheduleGenerator
    {

        private readonly TeamGenerator _teamGenerator;
        private readonly MatchGenerator _matchGenerator;
        private readonly RuleSet _ruleSet;

        public SeasonScheduleGenerator(TeamGenerator teamGenerator, MatchGenerator matchGenerator, RuleSet ruleSet)
        {
            _teamGenerator = teamGenerator;
            _matchGenerator = matchGenerator;
            _ruleSet = ruleSet;
        }


        public async Task<Season> GenerateSeasonSchedule(Season season, List<Player> players)
        {

            var allTeamCombinations = await _teamGenerator.GenerateAllTeamCombinations(players);
            var allMatchCombinations = await _matchGenerator.GenerateAllMatchCombinations(allTeamCombinations);
            var bestScheduledMatches = await GenerateBestScheduledMatches(allMatchCombinations);

            season.Matches = bestScheduledMatches;

            return await Task.FromResult(season);
        }


        private async Task<List<Match>> GenerateBestScheduledMatches(List<Match> allMatchCombinations)
        {
            var scheduledMatches = new List<Match>();
            var faultyMatches = new List<Match>();

            // step 1: try to add perfect matches to the scheduledMatches (validation score == 0)
            scheduledMatches = await AddPerfectMatches(allMatchCombinations, scheduledMatches, faultyMatches);

            // step 2: if the amountOfMatches is not enough, try to add faulty matches with the lowest faultPercentage

            return await Task.FromResult(scheduledMatches);
        }


        private async Task<List<Match>> AddPerfectMatches(List<Match> allMatchCombinations, List<Match> scheduledMatches, List<Match> faultyMatches)
        {

            foreach (var match in allMatchCombinations)
            {
                var validationResult = _ruleSet.Validate(match, scheduledMatches);

                if (validationResult == 0)
                {

                    scheduledMatches.Add(match);
                }
                else
                {
                    faultyMatches.Add(match);
                }
            }

            return await Task.FromResult(scheduledMatches);
        }
    }
}
