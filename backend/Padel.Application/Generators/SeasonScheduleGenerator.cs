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


        public async Task<Season> Generate(Season season, List<Player> players)
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
            var bestMatchScheduleSet = await GenerateBestMatchSet(allMatchCombinations, season.AmountOfMatches, players);


            season = PrepareSeasonForSaving(season, bestMatchScheduleSet);

            return await Task.FromResult(season);
        }


        public async Task<List<Match>> GenerateBestMatchSet(List<Match> allMatchCombinations, int amountOfMatches, List<Player> playerList)
        {
            var allMatchSchedulesSets = await _matchGenerator.GenerateAllMatchSchedulesSets(allMatchCombinations, amountOfMatches, 1000);

            return await GetBestMatchSet(allMatchSchedulesSets, playerList);
        }

        private async Task<List<Match>> GetBestMatchSet(List<List<Match>> allMatchSchedulesSets, List<Player> playerList)
        {
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
                    lowestFaultPoints = faultPoints;
                    bestMatchSet = matchSet;
                }
            }

            return await Task.FromResult(bestMatchSet ?? new List<Match>());
        }

        // Function to prepare the season for saving
        public Season PrepareSeasonForSaving(Season season, List<Match> matchSet)
        {
            if (matchSet == null || matchSet.Count == 0)
            {
                throw new ArgumentException("Match set cannot be null or empty.");
            }

            season.Matches = matchSet.Select(m => new Match(m.Team1 , m.Team2)).ToList();

            // Ensure the amount of matches matches the match set
            if (season.AmountOfMatches != matchSet.Count)
            {
                season.AmountOfMatches = matchSet.Count;
            }

            // Extract all teams from the match set
            var teamsUsed = matchSet
                .SelectMany(match => new[] { match.Team1, match.Team2 })
                .Distinct()
                .ToList();

            season.Teams = teamsUsed;


            DateTime firstMatchDate = GetFirstMatchDate(season.StartDate, season.DayOfWeek);

            // Assign dates to each match
            for (int i = 0; i < season.Matches.Count; i++)
            {
                // Calculate the match date by adding 7 days for each match
                DateTime matchDate = firstMatchDate.AddDays(i * 7);
                season.Matches[i].MatchDate = matchDate; // Assign the calculated match date
            }

            return season;
        }
        private DateTime GetFirstMatchDate(DateTime startDate, DayOfWeek matchDay)
        {
            // Calculate days until the next occurrence of the specified day of the week
            int daysUntilNextMatch = ((int)matchDay - (int)startDate.DayOfWeek + 7) % 7;
            if (daysUntilNextMatch == 0) // If it's the same day
            {
                return startDate; // Match is on the start date
            }
            return startDate.AddDays(daysUntilNextMatch); // Return the next occurrence
        }







    }
}
