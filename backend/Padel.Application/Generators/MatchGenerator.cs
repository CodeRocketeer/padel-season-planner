using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Generators
{
    public class MatchGenerator
    {
        public async Task<List<Match>> GenerateAllMatchCombinations(List<Team> teams)
        {
            if (teams == null || teams.Count < 2)
            {
                throw new ArgumentException("At least two teams are required to form matches.");
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
    }
}
