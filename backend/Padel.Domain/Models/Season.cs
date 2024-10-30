

namespace Padel.Domain.Models;

public class Season
{
    public int Id { get; set; }
    public int AmountOfMatches { get; private set; }
    public DateTime StartDate { get; private set; }
    public string Title { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public List<Team> Teams { get; set; } = [];
    public List<Match> Matches { get; set; } = [];

    public Season(int amountOfMatches, DateTime startDate, string title, DayOfWeek dayOfWeek)
    {

        if (!IsValidSeason(amountOfMatches, title))
        {
            throw new ArgumentException("Invalid season.");
        }

        AmountOfMatches = amountOfMatches;
        StartDate = startDate;
        Title = title;
        DayOfWeek = dayOfWeek;
    }


    private static bool IsValidSeason(int amountOfMatches, string title)
    {
        return amountOfMatches > 0 && !string.IsNullOrWhiteSpace(title);
    }

}
