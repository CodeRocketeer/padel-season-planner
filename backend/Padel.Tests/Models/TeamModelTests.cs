using Padel.Domain.Models;
namespace Padel.Tests.Models
{
    public class SeasonModelTests
    {
        [Fact]
        public void Season_Should_Have_RequiredProperties()
        {
            // Arrange
            var season = new Season
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.Now,
                Name = "Season 1",
                AmountOfMatches = 10,
                DayOfWeek = 2 // Valid DayOfWeek (Tuesday)
            };

            // Act & Assert
            Assert.NotEqual(Guid.Empty, season.Id);
            Assert.Equal("Season 1", season.Name);
            Assert.Equal(10, season.AmountOfMatches);
            Assert.Equal(2, season.DayOfWeek);
        }

        [Fact]
        public void Season_Should_CalculateEndDate_Correctly()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1); // Monday
            var season = new Season
            {  
                Id = Guid.NewGuid(),
                Name = "Season 1",
                StartDate = startDate,
                AmountOfMatches = 5 // 5 matches, one per week
            };

            // Act
            var endDate = season.EndDate;

            // Assert
            var expectedEndDate = startDate.AddDays((5 - 1) * 7);
            Assert.Equal(expectedEndDate, endDate);
        }

        [Theory]
        [InlineData(-1)] // Invalid DayOfWeek
        [InlineData(7)]  // Invalid DayOfWeek
        public void Season_Should_ThrowArgumentOutOfRangeException_OnInvalidDayOfWeek(int invalidDay)
        {
            // Arrange
            var season = new Season
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.Now,
                Name = "Season 1",
                AmountOfMatches = 10
            };

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => season.DayOfWeek = invalidDay);
        }


    }
}
