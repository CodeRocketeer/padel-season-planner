using Padel.Application.Services.Interfaces;
using Padel.Domain.Models;

namespace Padel.Application.Services;

public class MatchService : IMatchService
{
    public async Task<IEnumerable<Match>> GenerateAllMatchCombinations(List<Team> teams)
    {
        if (teams == null || teams.Count < 2)
        {
            throw new ArgumentException("At least two teams are required to form matches.");
        }

        var combinations = new List<Match>();

        for (int i = 0; i < teams.Count; i++)
        {
            for (int j = i + 1; j < teams.Count; j++)
            {
                try
                {
                    var match = new Match(teams[i], teams[j]);
                    combinations.Add(match);
                }
                catch (ArgumentException)
                {
                    // Ignore the match creation if teams have common players or same ID
                    continue;
                }
            }
        }

        return await Task.FromResult(combinations);
    }
}
