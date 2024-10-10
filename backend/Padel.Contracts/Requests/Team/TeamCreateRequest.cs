using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Team
{
    public class TeamCreateRequest
    {

        [Required(ErrorMessage = "MatchId is required.")]
        public Guid MatchId { get; init; }

        [Required(ErrorMessage = "Player1Id is required.")]
        public Guid Player1Id { get; init; }

        [Required(ErrorMessage = "Player2Id is required.")]
        public Guid Player2Id { get; init; }

        // Method to validate player IDs (this could also be implemented in a service)
        public void ValidatePlayerIds(List<Guid> existingPlayerIds)
        {
            if (!existingPlayerIds.Contains(Player1Id))
            {
                throw new ArgumentException($"Player ID {Player1Id} does not exist.");
            }

            if (!existingPlayerIds.Contains(Player2Id))
            {
                throw new ArgumentException($"Player ID {Player2Id} does not exist.");
            }
        }



    }
}
