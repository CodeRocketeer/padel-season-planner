using Padel.Application.Rules;
using Padel.Domain.Models;

public class GenderBalanceRule : IRule
{
    public int Weight => 90; // Assign a weight for this rule

    public int Validate(List<Match> matchSet, List<Player> playerList)
    {
        if (matchSet == null || !matchSet.Any())
        {
            return 100; // Max fault if there are no matches
        }

        int violationCount = 0;

        // Iterate through each match to check for gender balance
        foreach (var match in matchSet)
        {
            if (IsCompleteImbalance(match.Team1, match.Team2))
            {
                violationCount += 2; // Complete imbalance
            }
            else if (IsPartialImbalance(match.Team1, match.Team2))
            {
                violationCount += 1; // Partial imbalance
            }
        }

        return violationCount;
    }

    private static bool IsCompleteImbalance(Team team1, Team team2)
    {
        return AreBothTeamsSameGender(team1, Gender.Female) && AreBothTeamsSameGender(team2, Gender.Male) ||
               AreBothTeamsSameGender(team1, Gender.Male) && AreBothTeamsSameGender(team2, Gender.Female);
    }

    private static bool IsPartialImbalance(Team team1, Team team2)
    {
        return (IsMixedGenderTeam(team1) && !IsMixedGenderTeam(team2)) ||
               (!IsMixedGenderTeam(team1) && IsMixedGenderTeam(team2));
    }

    private static bool AreBothTeamsSameGender(Team team, Gender gender)
    {
        return team.Player1.Gender == gender && team.Player2.Gender == gender;
    }

    private static bool IsMixedGenderTeam(Team team)
    {
        return team.Player1.Gender != team.Player2.Gender;
    }
}