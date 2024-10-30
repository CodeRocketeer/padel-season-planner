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
