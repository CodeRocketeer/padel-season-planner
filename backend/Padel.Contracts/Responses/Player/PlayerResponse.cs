using Padel.Domain.Models;

namespace PadelContracts.Responses.Player
{
    public class PlayerResponse
    {
        public int Id { get; init; }
        public Guid UserId { get; init; }
        public Gender Gender { get; init; }
        public string Name { get; init; }



    }
}
