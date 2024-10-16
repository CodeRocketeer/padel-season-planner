namespace Padel.Domain.Models
{

    public partial class Season
    {
        private int _dayOfWeek;
        public required Guid Id { get; set; }
        public required DateTime StartDate { get; set; }

        public required string? Name { get; set; }
        public required int AmountOfMatches { get; set; }


        public List<Match> Matches { get; set; } = new();

        public List<Player> Players { get; set; } = new();

        // Computed property that calculates the end date based on the start date and amount of matches.
        public DateTime EndDate => CalculateEndDate();

        // Private method to compute the end date based on your logic.
        private DateTime CalculateEndDate()
        {
            // Assuming each match occurs weekly, and AmountOfMatches defines the total number of matches
            int matchIntervalInDays = 7; // For example, one match per week
            return StartDate.AddDays((AmountOfMatches - 1) * matchIntervalInDays);
        }

        public int DayOfWeek
        {
            get => _dayOfWeek;
            set
            {
                if (value < 0 || value > 6) // Validating against the valid DayOfWeek range
                {
                    throw new ArgumentOutOfRangeException(nameof(DayOfWeek), "DayOfWeek must be between 0 and 6.");
                }
                _dayOfWeek = value;
            }
        }

    }
}
