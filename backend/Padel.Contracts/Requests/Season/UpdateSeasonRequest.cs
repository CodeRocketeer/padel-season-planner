namespace Padel.Contracts.Requests.Season;

public class UpdateSeasonRequest
{
    public int AmountOfMatches { get; set; }
    public DateTime StartDate { get; set; }
    public string Title { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}
