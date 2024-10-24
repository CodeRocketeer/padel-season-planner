using FluentValidation;
using Padel.Application.Models;
using Padel.Application.Repositories;
using Padel.Application.Services.Interfaces;

namespace Padel.Application.Services;

public class TeamService : ITeamService
{
    private readonly IValidator<Team> _teamValidator;

    public TeamService( IValidator<Team> teamValidator)
    {
        _teamValidator = teamValidator;
    }

    public async Task<IEnumerable<Team>> CreateTeamCombinationsForSeasonAsync(Guid seasonId, IEnumerable<Participant> participants, CancellationToken token = default)
    {
        var teams = new List<Team>();
        var playerList = participants.ToList();

        // Generate all unique pairs of players
        for (int i = 0; i < playerList.Count; i++)
        {
            for (int j = i + 1; j < playerList.Count; j++)
            {
                var team = new Team(playerList[i], playerList[j])
                {
                    Id = Guid.NewGuid(),
                    SeasonId = seasonId
                };

                teams.Add(team);
            }
        }



        // Validate that the number of teams generated matches the expected number
        if (!ValidateTeamCountMatchesExpectedCount(playerList.Count, teams))
        {
            throw new InvalidOperationException("Invalid number of teams generated.");
        }

        // Validate each team with FluentValidation
        foreach (var team in teams)
        {
            await _teamValidator.ValidateAndThrowAsync(team);
        }

       return teams;
    }


    private bool ValidateTeamCountMatchesExpectedCount(int playerCount, List<Team> teams)
    {
        // Calculate the expected number of teams using the combination formula: C(n, 2) = n(n-1)/2
        int expectedTeamCount = (playerCount * (playerCount - 1)) / 2;

        // Check if the generated number of teams matches the expected number
        return teams.Count == expectedTeamCount;
    }





}
