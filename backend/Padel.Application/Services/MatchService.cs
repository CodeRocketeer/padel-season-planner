using Padel.Application.Models;
using Padel.Application.Rules;
using Padel.Application.Services.Interfaces;

namespace Padel.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly RuleSet _ruleSet;


        public MatchService(RuleSet ruleSet)
        {
            _ruleSet = ruleSet;
        }

        public async Task<IEnumerable<Match>> CreateBalancedMatchesForSeasonAsync(IEnumerable<Team> teams, Season season, IEnumerable<Participant> participants, CancellationToken token)
        {
            var scheduledMatchSets = new List<(List<Match> Matches, decimal FaultPercentage)>();

            // Number of iterations to try
            int iterations = 200;
            // Generate all possible matches for this iteration
            var allPossibleMatches = GeneratePossibleMatches(teams, season.Id);

            // Run multiple iterations to find the best set of planned matches
            for (int i = 0; i < iterations; i++)
            {
                // Shuffle the possible matches to randomize the selection for this iteration
                var shuffledMatches = ShuffleMatches(allPossibleMatches.ToList());
                // Call the function to generate matches for this iteration
                var (scheduledMatches, averageFaultPercentage) = CreateMatchesForSeasonIteration(teams, season, participants, shuffledMatches, token);

                // Add the planned matches to the list of all planned matches
                scheduledMatchSets.Add((scheduledMatches, averageFaultPercentage));
            }

            var sortedScheduledMatchSets = scheduledMatchSets.OrderBy(x => x.FaultPercentage).ToList();
            // Now evaluate all generated schedules and find the one with the lowest fault percentage
            var optimalScheduledMatchSet = sortedScheduledMatchSets.FirstOrDefault();  // Returns the tuple with the lowest fault percentage

            // TODO: add a matchDate to each match, using the startDate and the day of the week
            if (optimalScheduledMatchSet.Matches.Count == season.AmountOfMatches)
            {
                var updatedMatches = AssignMatchDates(optimalScheduledMatchSet.Matches, season.StartDate, season.DayOfWeek);
                return updatedMatches.AsEnumerable();
            }

            // Return empty if no optimal match set is found
            return Enumerable.Empty<Match>();
        }

        private (List<Match> scheduledMatches, decimal averageFaultPercentage) CreateMatchesForSeasonIteration(IEnumerable<Team> teams, Season season, IEnumerable<Participant> participants, List<Match> shuffledMatches, CancellationToken token)
        {
            var scheduledMatches = new List<Match>();
            var faultyMatches = new List<(Match match, decimal faultPercentage)>();

            // Step 1: Try to add perfect matches (validation score == 0)
            scheduledMatches = AddPerfectMatches(shuffledMatches, scheduledMatches, faultyMatches, season, participants.ToList(), token);

            // Step 2: If we don't have enough perfect matches, take from faulty matches
            if (scheduledMatches.Count < season.AmountOfMatches && faultyMatches.Count > 0)
            {
                scheduledMatches = AddFaultyMatches(scheduledMatches, faultyMatches, season, token);
            }

            // Calculate the average fault percentage for this iteration
            decimal averageFaultPercentage = CalculateAverageFaultPercentage(faultyMatches);

            // Return both the planned matches and the average fault percentage
            return (scheduledMatches, averageFaultPercentage);
        }

        private static List<Match> ShuffleMatches(List<Match> allPossibleMatches)
        {
            var random = new Random();
            var n = allPossibleMatches.Count;

            for (int i = n - 1; i > 0; i--)
            {
                // Pick a random index from 0 to i
                int j = random.Next(0, i + 1);

                // Swap possibleMatches[i] with the element at random index
                var temp = allPossibleMatches[i];
                allPossibleMatches[i] = allPossibleMatches[j];
                allPossibleMatches[j] = temp;
            }

            return allPossibleMatches;
        }

        private static decimal CalculateAverageFaultPercentage(List<(Match match, decimal faultPercentage)> faultyMatches)
        {
            // If there are no faulty matches, return 0 to avoid division by zero
            if (faultyMatches == null || faultyMatches.Count == 0)
            {
                return 0m;
            }

            // Calculate the sum of all fault percentages
            decimal totalFaultPercentage = faultyMatches.Sum(fm => fm.faultPercentage);

            // Calculate and return the average fault percentage
            return totalFaultPercentage / faultyMatches.Count;
        }


        private List<Match> AddPerfectMatches(IEnumerable<Match> shuffledMatches, List<Match> scheduledMatches, List<(Match match, decimal faultPercentage)> faultyMatches, Season season, List<Participant> participants, CancellationToken token)
        {


            foreach (var match in shuffledMatches)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                var validationResult = _ruleSet.Validate(match, scheduledMatches, participants);

                if (validationResult == 0)
                {
                    // Perfect match, store in scheduled matches
                    scheduledMatches.Add(match);
                }
                else
                {
                    faultyMatches.Add((match, validationResult));
                }

                if (scheduledMatches.Count >= season.AmountOfMatches)
                {
                    break; // Stop if we have enough matches
                }
            }
            return scheduledMatches;
        }


        private static List<Match> AddFaultyMatches(List<Match> scheduledMatches, List<(Match match, decimal faultPercentage)> faultyMatches, Season season, CancellationToken token)
        {
            var sortedFaultyMatches = faultyMatches.OrderBy(m => m.faultPercentage).ToList();

            foreach (var (match, faultPercentage) in sortedFaultyMatches)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                scheduledMatches.Add(match);

                if (scheduledMatches.Count >= season.AmountOfMatches)
                {
                    break; // Stop if we have enough matches
                }
            }

            return scheduledMatches;
        }

        private List<Match> AssignMatchDates(List<Match> matches, DateTime startDate, int dayOfWeek)
        {
            // Step 1: Get the first match date based on season start date and day of the week
            DateTime nextMatchDate = GetFirstMatchDate(startDate, dayOfWeek);

            // Step 2: Assign the match dates to each match, one week apart
            foreach (var match in matches)
            {
                match.MatchDate = nextMatchDate;  // Assign the calculated match date
                nextMatchDate = nextMatchDate.AddDays(7);  // Move to the next week
            }

            // Return the updated list of matches
            return matches;
        }


        private static DateTime GetFirstMatchDate(DateTime startDate, int dayOfWeek)
        {
            // Calculate the number of days until the desired day of the week
            int daysUntilMatchDay = ((dayOfWeek - (int)startDate.DayOfWeek + 7) % 7);

            // Return the first match date, which is the start date plus the calculated number of days
            return startDate.AddDays(daysUntilMatchDay);
        }


        // Method to generate combinations explicitly
        private static IEnumerable<Match> GeneratePossibleMatches(IEnumerable<Team> teams, Guid seasonId)
        {
            var possibleMatches = new List<Match>();
            var teamList = teams.ToList();
            int n = teamList.Count;

            // Loop through each unique pair of teams
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    var team1 = teamList[i];
                    var team2 = teamList[j];

                    // Create the match if participants are unique
                    var match = new Match(team1, team2)
                    {
                        Id = Guid.NewGuid(),
                        SeasonId = seasonId
                    };

                    possibleMatches.Add(match);

                }
            }

            return possibleMatches;
        }

    }



}


