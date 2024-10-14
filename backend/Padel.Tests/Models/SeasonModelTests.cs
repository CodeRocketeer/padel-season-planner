namespace Padel.Tests.Models
{
    public class SeasonModelTests
    {
        [Fact]
        public void Season_Should_Set_Properties_Correctly()
        {
            // Arrange
            var seasonId = Guid.NewGuid();
            var startDate = new DateTime(2024, 1, 1);
            var amountOfMatches = 5;
            var seasonName = "2024 Season";
            var dayOfWeek = (int)DayOfWeek.Monday; // Assuming DayOfWeek is stored as an int

            // Act
            var season = new Season
            {
                Id = seasonId,
                StartDate = startDate,
                AmountOfMatches = amountOfMatches,
                Name = seasonName,
                DayOfWeek = dayOfWeek
            };

            // Assert
            Assert.Equal(seasonId, season.Id);
            Assert.Equal(startDate, season.StartDate);
            Assert.Equal(amountOfMatches, season.AmountOfMatches);
            Assert.Equal(seasonName, season.Name);
            Assert.Equal(dayOfWeek, season.DayOfWeek);


        }

        [Fact]
        public void Season_Should_Throw_Exception_When_DayOfWeek_Is_Out_Of_Range()
        {
            // Arrange
            var seasonId = Guid.NewGuid();
            var startDate = new DateTime(2024, 1, 1);
            var amountOfMatches = 5;
            var dayOfWeek = 7;
            var name = "2024 Season";


            Assert.Throws<ArgumentOutOfRangeException>(() => new Season
            {
                Id = seasonId,
                StartDate = startDate,
                AmountOfMatches = amountOfMatches,
                Name = name,
                DayOfWeek = dayOfWeek
            });


        }


    }
}
