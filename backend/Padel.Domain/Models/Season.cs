

namespace Padel.Domain.Models;

public class Season
{
    public int Id { get; set; }
    public int AmountOfMatches { get; private set; }
    public DateTime StartDate { get;private  set; }
    public string Title { get;private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public List<Team> Teams { get; set; }
    public List<Match> Matches { get; set; }

    public Season(int amountOfMatches, DateTime startDate, string title, DayOfWeek dayOfWeek)
    {


        if (amountOfMatches <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountOfMatches), "Amount of matches must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw  new ArgumentNullException(nameof(title), "Title cannot be null.");
        }


        AmountOfMatches = amountOfMatches;
        StartDate = startDate;
        Title = title;
        DayOfWeek = dayOfWeek; 
     
    }



    
}
