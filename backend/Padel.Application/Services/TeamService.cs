using Padel.Application.Models;
using Padel.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services;

public class TeamService : ITeamService
{
    public async Task<IEnumerable<Team>> GenerateAllTeamCombinations(List<Player> players)
    {
        if (players == null || players.Count < 2)
        {
            throw new ArgumentException("At least two players are required to form teams.");
        }

        var combinations = new List<Team>();
        int teamId = 1;  // Start IDs from 1 or any other positive number

        for (int i = 0; i < players.Count; i++)
        {
            for (int j = i + 1; j < players.Count; j++)
            {
                var team = new Team(players[i], players[j]) { Id = teamId++ };  // Assign unique ID
                combinations.Add(team);
            }
        }

        return await Task.FromResult(combinations);
    }
}
