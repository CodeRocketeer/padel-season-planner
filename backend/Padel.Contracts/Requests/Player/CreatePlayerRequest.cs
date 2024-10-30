using Padel.Domain.Models;
namespace PadelContracts.Requests.Player;

public class CreatePlayerRequest
{
    public Gender Gender { get; set; }

    public string Name { get; set; }


}
