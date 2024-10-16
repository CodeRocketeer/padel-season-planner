using FluentValidation;
using Padel.Application.Repositories;
using Padel.Domain.Models;

namespace Padel.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IValidator<Match> _matchValidator;

        public MatchService(IMatchRepository matchRepository, ISeasonRepository seasonRepository, IValidator<Match> matchValidator)
        {
            _matchValidator = matchValidator;
            _matchRepository = matchRepository;
            _seasonRepository = seasonRepository;
        }

        public async Task<bool> CreateAsync(Match match, CancellationToken token = default)
        {
            // Match validation occurs in the Match model
            await _matchValidator.ValidateAndThrowAsync(match, token);
            // Check if the specified season exists
            var seasonExists = await _seasonRepository.ExistsByIdAsync(match.SeasonId, token);
            if (!seasonExists)
            {
                throw new InvalidOperationException("Cannot create match. The specified season does not exist.");
            }
            // Create the match
            return await _matchRepository.CreateAsync(match, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _matchRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Match>> GetAllAsync(CancellationToken token = default)
        {
            return _matchRepository.GetAllAsync(token);
        }

        public Task<Match?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _matchRepository.GetByIdAsync(id, token);
        }

        public async Task<Match?> UpdateAsync(Match match, CancellationToken token = default)
        {
            // Match validation occurs in the Match model
            await _matchValidator.ValidateAndThrowAsync(match, token);
            // Check if the specified match exists
            var matchExists = await _matchRepository.ExistsByIdAsync(match.Id, token);
            if (!matchExists) return null;

            // Update the match
            await _matchRepository.UpdateAsync(match, token);
            return match;
        }

        public async Task<IEnumerable<Match>> GetAllBySeasonIdAsync(Guid seasonId, CancellationToken token = default)
        {
            return await _matchRepository.GetBySeasonIdAsync(seasonId, token);
        }


        private static DateTime GetFirstMatchDate(DateTime startDate, int dayOfWeek)
        {
            // Get the difference between the current day of the week and the desired match day
            int daysUntilMatchDay = ((dayOfWeek - (int)startDate.DayOfWeek + 7) % 7);

            // Return the first match date, which is the start date plus the calculated difference
            return startDate.AddDays(daysUntilMatchDay);
        }

        public async Task<IEnumerable<Match>> GenerateBalancedMatchesForSeason(List<Team> teams, Season season, List<Player> players, CancellationToken token = default)
        {
            var matches = new List<Match>();
            var firstMatchDate = GetFirstMatchDate(season.StartDate, season.DayOfWeek);
            var matchDate = firstMatchDate; // Initialize match date

            // Dictionary to track the number of matches each player has played
            var playerMatchCount = new Dictionary<Guid, int>();

            // Initialize player match count
            foreach (var player in players)
            {
                playerMatchCount[player.Id] = 0;
            }

            while (matches.Count < season.AmountOfMatches)
            {
                // Take two random teams from the list
                var team1 = await TakeRandomTeamFromListWithoutRepetition(teams, playerMatchCount);
                var team2 = await TakeRandomTeamFromListWithoutRepetition(teams, playerMatchCount);

                // Check if the two teams are not the same
                if (team1 == team2)
                    continue;

                // Check if the two teams have at least 1 player in common
                if (!HaveCommonPlayer(team1, team2))
                    continue;

                // Check if the teams are balanced based on player sex
                if (!AreTeamsBalancedBySex(team1, team2))
                    continue;

                // Check if any player from either team has played in the last match
                if (HasConsecutivePlayerParticipation(matches, team1, team2))
                    continue;

                // Create the match
                var match = new Match
                {
                    Id = Guid.NewGuid(),
                    Team1Id = team1.Id,
                    Team2Id = team2.Id,
                    MatchDate = matchDate,
                    SeasonId = season.Id,
                    Teams = new List<Team> { team1, team2 }
                };

                // Update player participation counts
                foreach (var player in team1.Players)
                {
                    playerMatchCount[player.Id]++;
                }
                foreach (var player in team2.Players)
                {
                    playerMatchCount[player.Id]++;
                }

                // Add the match to the list
                matches.Add(match);

                // Optionally increment match date for the next match (weekly increment in this case)
                matchDate = matchDate.AddDays(7);
            }

            Console.WriteLine($"Generated {matches.Count} matches for season {season.Id} and each player has played {playerMatchCount.Count.ToString()} matches");

            return matches;
        }

        public static Task<Team> TakeRandomTeamFromList(List<Team> teams)
        {
            var random = new Random();
            var team = teams[random.Next(teams.Count)];

            return Task.FromResult(team);
        }


        private static Task<Team> TakeRandomTeamFromListWithoutRepetition(List<Team> teams, Dictionary<Guid, int> playerMatchCount)
        {
            var random = new Random();

            // Check for teams with players who have not played yet
            var teamsWithUnplayedPlayers = teams
                .Where(team => team.Players.Any(player => playerMatchCount[player.Id] == 0))
                .ToList();

            Team selectedTeam;

            if (teamsWithUnplayedPlayers.Any())
            {
                // Select a random team that has at least one player who hasn't played yet
                selectedTeam = teamsWithUnplayedPlayers[random.Next(teamsWithUnplayedPlayers.Count)];
            }
            else
            {
                // No unplayed players, so we need to check for the least amount of matches played
                var leastPlayedTeams = teams
                    .OrderBy(t => t.Players.Min(p => playerMatchCount[p.Id])) // Get the minimum match count for players in each team
                    .ToList();

                // Find the minimum matches played across all teams
                int minMatchesPlayed = leastPlayedTeams.Min(t => t.Players.Min(p => playerMatchCount[p.Id]));

                // Filter teams that have the minimum matches played
                var teamsWithLeastMatches = leastPlayedTeams
                    .Where(t => t.Players.Min(p => playerMatchCount[p.Id]) == minMatchesPlayed)
                    .ToList();

                // Select a random team among those with the least matches played
                selectedTeam = teamsWithLeastMatches[random.Next(teamsWithLeastMatches.Count)];
            }

            return Task.FromResult(selectedTeam);
        }


        private static bool AreTeamsBalancedBySex(Team team1, Team team2)
        {
            // Count the number of male and female players in both teams
            int maleCountTeam1 = team1.Players.Count(m => m.Sex == "M");
            int femaleCountTeam1 = team1.Players.Count(m => m.Sex == "F");

            int maleCountTeam2 = team2.Players.Count(m => m.Sex == "M");
            int femaleCountTeam2 = team2.Players.Count(m => m.Sex == "F");

            // Check for balanced matches
            bool isMM = maleCountTeam1 == 2 && maleCountTeam2 == 2; // MM vs MM
            bool isFF = femaleCountTeam1 == 2 && femaleCountTeam2 == 2; // FF vs FF
            bool isMF = maleCountTeam1 == 1 && femaleCountTeam1 == 1 && maleCountTeam2 == 1 && femaleCountTeam2 == 1; // MF vs MF

            return isMM || isFF || isMF;
        }

        // Function to check if two teams have at least one player in common
        private static bool HaveCommonPlayer(Team team1, Team team2)
        {
            return team1.Players.Intersect(team2.Players).Any();
        }


        // Function to check if any player from either team has played in the last match
        private bool HasConsecutivePlayerParticipation(List<Match> matches, Team team1, Team team2)
        {
            // Check if there are any matches already created
            if (matches.Count == 0)
                return false;

            // Get the last match
            var lastMatch = matches.Last();

            // Check if players from team1 or team2 participated in the last match
            bool team1PlayedLastMatch = lastMatch.Team1Id == team1.Id || lastMatch.Team2Id == team1.Id;
            bool team2PlayedLastMatch = lastMatch.Team1Id == team2.Id || lastMatch.Team2Id == team2.Id;

            // If either team played in the last match, check player participation
            if (team1PlayedLastMatch || team2PlayedLastMatch)
            {
                var playersInLastMatch = new HashSet<Player>(
                    (lastMatch.Team1Id == team1.Id ? team1.Players : team2.Players)
                    .Concat(lastMatch.Team1Id == team2.Id ? team2.Players : team1.Players)
                );

                // Check if any of the players from the current teams are in the last match
                return playersInLastMatch.Intersect(team1.Players).Any() || playersInLastMatch.Intersect(team2.Players).Any();
            }

            return false; // No players played consecutively
        }








    }
}
