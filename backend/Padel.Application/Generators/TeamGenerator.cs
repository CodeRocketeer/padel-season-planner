using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Generators
{
    public class TeamGenerator
    {
        public async Task<List<Team>> GenerateAllTeamCombinations(List<Player> players)
        {
            if (players == null || players.Count < 2)
            {
                throw new ArgumentException("At least two players are required to form teams.");
            }

            var combinations = new List<Team>();
            int teamId = 1;

            // Outer loop through all players
            foreach (var player in players)
            {
                // Inner loop to pair with players not yet paired with `player`
                foreach (var otherPlayer in players.Skip(players.IndexOf(player) + 1))
                {
                    if (Team.IsValidTeam(player, otherPlayer))
                    {
                        combinations.Add(new Team(player, otherPlayer)
                        {
                            Id = teamId
                        });
                        teamId++;
                    }
                }
            }

            return await Task.FromResult(combinations);
        }

    }
}
