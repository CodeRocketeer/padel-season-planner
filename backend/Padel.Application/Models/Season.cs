using Padel.Application.Database.Entities;
using System.Xml.Linq;

namespace Padel.Application.Models
{
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



        public static SeasonEntity ToEntity(Season season)
        {
            if (season == null)
            {
                throw new ArgumentNullException(nameof(season), "Season cannot be null.");
            }

            return new SeasonEntity
            {
                Id = season.Id != 0 ? season.Id : 0,
                AmountOfMatches = season.AmountOfMatches,
                DayOfWeek = season.DayOfWeek,
                StartDate = DateTime.SpecifyKind(season.StartDate, DateTimeKind.Utc), // Ensure UTC kind
                Title = season.Title
            };
        }

        public static Season FromEntity(SeasonEntity seasonEntity)
        {
            if (seasonEntity == null)
            {
                throw new ArgumentNullException(nameof(seasonEntity), "Season entity cannot be null.");
            }

            return new Season(seasonEntity.AmountOfMatches, seasonEntity.StartDate, seasonEntity.Title, seasonEntity.DayOfWeek)
            {
                Id = seasonEntity.Id,
                        

            };
        }
    }
}
