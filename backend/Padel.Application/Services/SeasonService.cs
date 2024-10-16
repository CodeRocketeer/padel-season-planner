using FluentValidation;
using Padel.Application.Repositories;
using Padel.Domain.Models;

namespace Padel.Application.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IValidator<Season> _seasonValidator;
        private readonly IMatchService _matchService;
        private readonly IMatchRepository _matchRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<Team> _teamValidator;
        private readonly IValidator<Match> _matchValidator;

        public SeasonService(ISeasonRepository seasonRepository, IMatchService matchService, IMatchRepository matchRepository, IValidator<Season> seasonValidator, IPlayerRepository playerRepository, ITeamRepository teamRepository, IValidator<Team> teamValidator, IValidator<Match> matchValidator)
        {
            _seasonRepository = seasonRepository;
            _matchService = matchService;
            _seasonValidator = seasonValidator;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _teamValidator = teamValidator;
            _matchValidator = matchValidator;
        }

        public async Task<bool> CreateAsync(Season season, CancellationToken token = default)
        {
            // Validate and create the season first
            await _seasonValidator.ValidateAndThrowAsync(season, token);
            var isCreated = await _seasonRepository.CreateAsync(season, token);

            if (!isCreated) return false;
            return true;
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _seasonRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Season>> GetAllAsync(CancellationToken token = default)
        {
            return _seasonRepository.GetAllAsync(token);
        }

        public Task<Season?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _seasonRepository.GetByIdAsync(id, token);
        }

        public async Task<Season?> UpdateAsync(Season season, CancellationToken token = default)
        {
            // Season validation occurs in the Season model
            await _seasonValidator.ValidateAndThrowAsync(season, token);
            var seasonExists = await _seasonRepository.ExistsByIdAsync(season.Id, token);
            if (!seasonExists) return null;

            await _seasonRepository.UpdateAsync(season, token);
            return season;
        }



        private DateTime GetFirstMatchDate(DateTime startDate, int dayOfWeek)
        {
            // Get the difference between the current day of the week and the desired match day
            int daysUntilMatchDay = ((dayOfWeek - (int)startDate.DayOfWeek + 7) % 7);

            // Return the first match date, which is the start date plus the calculated difference
            return startDate.AddDays(daysUntilMatchDay);
        }


        private void ValidateGeneratedTeams(int playerCount, List<Team> teams)
        {
            // Calculate the expected number of teams using the combination formula: C(n, 2) = n(n-1)/2
            int expectedTeamCount = (playerCount * (playerCount - 1)) / 2;

            // Check if the generated number of teams matches the expected number
            if (teams.Count != expectedTeamCount)
            {
                throw new InvalidOperationException($"Invalid number of teams generated. Expected {expectedTeamCount} teams, but generated {teams.Count} teams.");
            }
        }

        // Generate all unique pairs of players to create teams (CHECKED)
        private List<Team> GenerateTeamsForSeason(Season season, IEnumerable<Player> players)
        {
            var teams = new List<Team>();
            var playerList = players.ToList();

            // Generate all unique pairs of players
            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = i + 1; j < playerList.Count; j++)
                {
                    var team = new Team
                    {
                        Id = Guid.NewGuid(),
                        SeasonId = season.Id,
                        Player1Id = playerList[i].Id,
                        Player2Id = playerList[j].Id,
                        Players = new List<Player>() { playerList[i], playerList[j] }
                    };

                    teams.Add(team);
                }
            }

            // Validate the number of generated teams
            ValidateGeneratedTeams(playerList.Count, teams);

            return teams;
        }

        private bool ValidatePlayerParticipationBalanced(Dictionary<Guid, int> playerParticipationCounts, int tolerance = 0)
        {
            // Get the minimum and maximum participation counts
            int minParticipation = playerParticipationCounts.Values.Min();
            int maxParticipation = playerParticipationCounts.Values.Max();

            // Check if the participation counts are within the allowed tolerance
            if (maxParticipation - minParticipation > tolerance)
            {
                Console.WriteLine($"Unbalanced participation detected: Min = {minParticipation}, Max = {maxParticipation}");
                return false;
            }

            return true;
        }

        private async Task<List<Match>> GenerateBalancedMatchesForSeason(List<Team> teams, Season season, List<Player> players)
        {
            var matches = new List<Match>();
            var amountOfMatches = season.AmountOfMatches;
            var firstMatchDate = GetFirstMatchDate(season.StartDate, season.DayOfWeek);

            // Classify teams by gender composition
            var maleMaleTeams = teams.Where(t => t.Players.All(p => p.Sex == "M")).ToList();
            var femaleFemaleTeams = teams.Where(t => t.Players.All(p => p.Sex == "F")).ToList();
            var mixedTeams = teams.Where(t => t.Players.Any(p => p.Sex == "M") && t.Players.Any(p => p.Sex == "F")).ToList();

            // Track player participation counts
            var playerParticipationCounts = players.ToDictionary(player => player.Id, player => 0);

            var random = new Random();

            // Generate matches until we reach the desired amount
            for (int i = 0; i < amountOfMatches; i++)
            {
                Team team1 = null;
                Team team2 = null;

                // Randomly select which type of match to generate (m,m), (f,f), or (m,f)
                var matchType = random.Next(3); // 0 = (m,m), 1 = (f,f), 2 = (m,f)

                // Pick teams with the least participation for this match type
                if (matchType == 0 && maleMaleTeams.Count >= 2) // Male vs Male
                {
                    (team1, team2) = SelectBalancedTeams(maleMaleTeams, matches, playerParticipationCounts);
                }
                else if (matchType == 1 && femaleFemaleTeams.Count >= 2) // Female vs Female
                {
                    (team1, team2) = SelectBalancedTeams(femaleFemaleTeams, matches, playerParticipationCounts);
                }
                else if (matchType == 2 && mixedTeams.Count >= 2) // Mixed Gender
                {
                    (team1, team2) = SelectBalancedTeams(mixedTeams, matches, playerParticipationCounts);
                }

                // If no valid teams are found, continue to the next iteration
                if (team1 == null || team2 == null)
                {
                    i--; // Retry this iteration if we couldn't find a valid match
                    continue;
                }



                team1.Players.Add(players.First(p => p.Id == team1.Player1Id));
                team1.Players.Add(players.First(p => p.Id == team1.Player2Id));

                team2.Players.Add(players.First(p => p.Id == team2.Player1Id));
                team2.Players.Add(players.First(p => p.Id == team2.Player2Id));


                // Create the match
                var match = new Match
                {
                    Id = Guid.NewGuid(),
                    Team1Id = team1.Id,
                    Team2Id = team2.Id,
                    MatchDate = firstMatchDate.AddDays(i * 7), // Schedule one match per week
                    SeasonId = season.Id,
                    Teams = new List<Team>() { team1, team2 }
                };

                 
                await _matchValidator.ValidateAndThrowAsync(match, default);

                matches.Add(match);

                // Update player participation counts
                playerParticipationCounts[team1.Player1Id]++;
                playerParticipationCounts[team1.Player2Id]++;
                playerParticipationCounts[team2.Player1Id]++;
                playerParticipationCounts[team2.Player2Id]++;
            }



            //if (!ValidatePlayerParticipationBalanced(playerParticipationCounts, 1))
            //{
            //    Console.WriteLine("Error: Players do not have balanced participation across matches.");
            //    return null; // or handle the error appropriately
            //}

            return matches;
        }
       


        private (Team, Team) SelectBalancedTeams(List<Team> availableTeams, List<Match> existingMatches, Dictionary<Guid, int> playerParticipationCounts)
        {
            Team selectedTeam1 = null;
            Team selectedTeam2 = null;

            // Sort teams by the least number of participations (to balance player participation)
            var sortedTeams = availableTeams
                .OrderBy(t => playerParticipationCounts[t.Player1Id] + playerParticipationCounts[t.Player2Id])
                .ToList();

            // Try to find a valid match
            for (int i = 0; i < sortedTeams.Count; i++)
            {
                var team1 = sortedTeams[i];

                for (int j = i + 1; j < sortedTeams.Count; j++)
                {
                    var team2 = sortedTeams[j];

                    // Check if these two teams already played against each other
                    bool alreadyPlayed = existingMatches.Any(m =>
                        (m.Team1Id == team1.Id && m.Team2Id == team2.Id) ||
                        (m.Team1Id == team2.Id && m.Team2Id == team1.Id));

                    // Ensure teams do not share the same players
                    bool noPlayerOverlap = team1.Player1Id != team2.Player1Id &&
                                           team1.Player1Id != team2.Player2Id &&
                                           team1.Player2Id != team2.Player1Id &&
                                           team1.Player2Id != team2.Player2Id;

                    if (!alreadyPlayed && noPlayerOverlap)
                    {
                        selectedTeam1 = team1;
                        selectedTeam2 = team2;
                        break;
                    }
                }

                if (selectedTeam1 != null && selectedTeam2 != null)
                {
                    break;
                }
            }

            return (selectedTeam1, selectedTeam2);
        }




        public async Task<bool> PopulateSeasonAsync(Guid id, CancellationToken token)
        {
            // Check if the season exists
            var season = await _seasonRepository.GetByIdAsync(id, token);
            if (season == null) return false;

            // Get players and generate teams
            var players = await _playerRepository.GetBySeasonIdAsync(id, token);
            if (players == null || !players.Any()) return false;

            var teams = GenerateTeamsForSeason(season, players);
            var matches = await GenerateBalancedMatchesForSeason(teams, season, players.ToList());

            var usedTeamIds = matches.SelectMany(m => new[] { m.Team1Id, m.Team2Id }).Distinct().ToList();
            var usedTeams = teams.Where(t => usedTeamIds.Contains(t.Id)).ToList();

            // Validate and save the used teams
            foreach (var team in usedTeams)
            {
                await _teamValidator.ValidateAndThrowAsync(team, token);
            }

            await _teamRepository.CreateManyAsync(usedTeams, token);
            await _matchRepository.CreateManyAsync(matches, token);

            return true;
        }

    }


}
