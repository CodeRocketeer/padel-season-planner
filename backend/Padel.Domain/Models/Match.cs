

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
        Team1 = team1 ?? throw new ArgumentNullException(nameof(team1));
        Team2 = team2 ?? throw new ArgumentNullException(nameof(team2));

        if (team1.Id == team2.Id)
        {
            throw new ArgumentException("Team1 and Team2 cannot have the same ID.", nameof(team1));
        }


        // Check for common players based on UserId
        if (team1.Player1.UserId == team2.Player1.UserId ||
            team1.Player1.UserId == team2.Player2.UserId ||
            team1.Player2.UserId == team2.Player1.UserId ||
            team1.Player2.UserId == team2.Player2.UserId)
        {
            throw new ArgumentException("Teams cannot have common players.");
        }
    }
}