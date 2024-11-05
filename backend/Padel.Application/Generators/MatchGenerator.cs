using Padel.Domain.Models;

namespace Padel.Application.Generators
{
    public class MatchGenerator
    {
        public async Task<List<Match>> GenerateAllMatchCombinations(List<Team> teams)
        {
            if (teams == null || teams.Count < 2)
            {
                return new List<Match>();
            }


            var possibleMatches = new List<Match>();

            foreach (var team1 in teams)
            {
                foreach (var team2 in teams.Skip(teams.IndexOf(team1) + 1))
                {
                    if (Match.IsValidMatch(team1, team2))
                    {
                        possibleMatches.Add(new Match(team1, team2));
                    }
                }
            }
            return await Task.FromResult(possibleMatches);
        }

        public async Task<List<List<Match>>> GenerateAllMatchSchedulesSets(List<Match> allMatchCombinations, int amountOfmatches, int numberOfRandomSets = 50)
        {
            // List of sets of scheduled matches
            var matchSchedulesSets = new List<List<Match>>();
            var random = new Random();

            for (int i = 0; i < numberOfRandomSets; i++)
            {
                var scheduledMatches = new List<Match>();

                for (int j = 0; j < amountOfmatches; j++)
                {
                    scheduledMatches.Add(GetRandomMatch(allMatchCombinations));
                }

                matchSchedulesSets.Add(scheduledMatches);

            }

            return await Task.FromResult(matchSchedulesSets);

        }

        private static Match GetRandomMatch(List<Match> allPossibleMatches)
        {


            var random = new Random();
            int randomIndex = random.Next(0, allPossibleMatches.Count);

            return allPossibleMatches[randomIndex];
        }

    }
}
