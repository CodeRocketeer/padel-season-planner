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
            if (players == null || players.Count < 2)
            {
                throw new ArgumentException("At least 2 players are required to generate a season schedule.");
            }

            // Calculate minimum matches required to ensure all players participate
            int minMatchesNeeded = (int)Math.Ceiling(players.Count / 4.0);
            season.AmountOfMatches = Math.Max(season.AmountOfMatches, minMatchesNeeded);


            var allTeamCombinations = await _teamGenerator.GenerateAllTeamCombinations(players);
            var allMatchCombinations = await _matchGenerator.GenerateAllMatchCombinations(allTeamCombinations);
            var bestMatchScheduleSet = await GenerateBestMatchScheduleSet(allMatchCombinations, season.AmountOfMatches, players);


            season.Matches = bestMatchScheduleSet;

            return await Task.FromResult(season);
        }


        public async Task<List<Match>> GenerateBestMatchScheduleSet(List<Match> allMatchCombinations, int amountOfMatches, List<Player> playerList)
        {
            var allMatchSchedulesSets = await _matchGenerator.GenerateAllMatchSchedulesSets(allMatchCombinations, amountOfMatches, 5000);

            List<Match> bestMatchSet = null;
            int lowestFaultPoints = int.MaxValue;

            foreach (var matchSet in allMatchSchedulesSets)
            {
                int faultPoints = _ruleSet.Validate(matchSet, playerList);

                if (faultPoints == 0)
                {
                    return matchSet;
                }
                if (faultPoints < lowestFaultPoints)
                {
                    lowestFaultPoints= faultPoints;
                    bestMatchSet = matchSet;
                }
            }

            return await Task.FromResult(bestMatchSet ?? new List<Match>());
        }




    }
}
