namespace Padel.Domain.Models;

public class Team
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    // Constructor allowing optional participants
    public Team(Player player1, Player player2)
    {

        Player1 = player1;
        Player2 = player2;

        if (!IsValidTeam(player1, player2))
        {
            throw new ArgumentException("Invalid team.");
        }

    }


    public static bool IsValidTeam(Player player1, Player player2) =>
        player1 != null && player2 != null && player1.UserId != player2.UserId;

    public List<Player> GetParticipants()
    {
        return new List<Player> { Player1, Player2 };
    }
}
