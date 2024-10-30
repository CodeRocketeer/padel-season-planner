

namespace Padel.Domain.Models;

public class Match
{
    public int Id { get; set; }
    public int SeasonId { get; set; }
    public Team Team1 { get; private set; }
    public Team Team2 { get; private set; }
    public DateTime MatchDate { get; set; }


    // Constructor allowing teams to be created with IDs only
    public Match(Team team1, Team team2)
    {

    
        Team1 = team1;
        Team2 = team2;

        if (!IsValidMatch(team1, team2))
        {
            throw new ArgumentException("Invalid match.");
        }
    }

    public static bool IsValidMatch(Team team1, Team team2)
    {
        if (team1 == null || team2 == null || team1 == team2 || team1.Id == team2.Id)
        {
            return false;
        }

        // Check if the teams are composed of different users
        if (!AreTeamsComposedOfDifferentUsers(team1, team2))
        {
            return false;
        }

        // Ensure all players are assigned
        if (team1.Player1 == null || team1.Player2 == null || team2.Player1 == null || team2.Player2 == null)
        {
            return false;
        }

        return AreTeamsGenderBalanced(team1, team2);
    }

    private static bool AreTeamsComposedOfDifferentUsers(Team team1, Team team2)
    {
        return !(team1.Player1?.UserId == team2.Player1?.UserId ||
                 team1.Player1?.UserId == team2.Player2?.UserId ||
                 team1.Player2?.UserId == team2.Player1?.UserId ||
                 team1.Player2?.UserId == team2.Player2?.UserId);
    }


    private static bool AreTeamsGenderBalanced(Team team1, Team team2)
    {
        // Ensure all players are present
        if (team1.Player1 == null || team1.Player2 == null || team2.Player1 == null || team2.Player2 == null)
        {
            return false;
        }

        // Determine the gender composition of each team
        var team1Genders = new[] { team1.Player1.Gender, team1.Player2.Gender };
        var team2Genders = new[] { team2.Player1.Gender, team2.Player2.Gender };

        // Check for gender balance conditions
        bool team1IsMixed = team1Genders.Contains(Gender.Female) && team1Genders.Contains(Gender.Male);
        bool team2IsMixed = team2Genders.Contains(Gender.Female) && team2Genders.Contains(Gender.Male);

        // A valid match must be:
        // 1. Both teams mixed
        // 2. Both teams single gender (MM or FF)
        return (team1IsMixed && team2IsMixed) ||
               (team1Genders[0] == team1Genders[1] && team1Genders[0] == team2Genders[0] && team1Genders[0] == team2Genders[1]);
    }
}
