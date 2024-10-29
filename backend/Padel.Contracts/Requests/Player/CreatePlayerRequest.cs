using Padel.Application.Models;

namespace PadelContracts.Requests.Player;

public class CreatePlayerRequest
{
    public Gender Gender { get; set; }

    public string Name { get; set; }


}
