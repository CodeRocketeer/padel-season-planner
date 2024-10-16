using System;

namespace Padel.Infrastructure.Entities
{
    public class PlayerEntity
    {
        public Guid Id { get; set; }

        public Guid SeasonId { get; set; }
        public Guid UserId { get; set; } // Foreign key to User
        public string Name { get; set; }
        public string Sex { get; set; }
    }
}
