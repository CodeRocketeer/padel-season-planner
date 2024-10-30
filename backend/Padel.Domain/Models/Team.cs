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
        Player1 = player1 ?? throw new ArgumentNullException(nameof(player1));
        Player2 = player2 ?? throw new ArgumentNullException(nameof(player2));

        if (player1.UserId == player2.UserId)
        {
            throw new ArgumentException("Player1 and Player2 cannot have the same user ID.", nameof(player1));
        }
    }

    

}
