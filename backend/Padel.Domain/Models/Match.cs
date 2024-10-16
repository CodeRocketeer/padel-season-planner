
namespace Padel.Domain.Models
{
    public partial class Match
    {
        public required Guid Id { get; set; }
        public required Guid SeasonId { get; set; } // This should match 'season_id' in the database
        public required DateTime MatchDate { get; set; }

        public required Guid Team1Id { get; set; } // This should match 'team1_id' in the database
        public required Guid Team2Id { get; set; } // This should match 'team2_id' in the database

        public List<Team> Teams { get; set; } = new List<Team>(); // This should match 'teams' in the database>

    }
}
