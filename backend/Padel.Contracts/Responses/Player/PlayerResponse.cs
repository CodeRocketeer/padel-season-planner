using Padel.Domain.Models;

namespace PadelContracts.Responses.Player
{
    public class PlayerResponse
    {
        public Guid UserId { get; init; }
        public Gender Gender { get; init; }
        public string Name { get; init; }



    }
}
