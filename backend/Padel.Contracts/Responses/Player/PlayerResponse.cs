using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Responses.Player
{
    public class PlayerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }

        public Guid UserId { get; set; }

        public Guid SeasonId { get; set; }
    }
}
