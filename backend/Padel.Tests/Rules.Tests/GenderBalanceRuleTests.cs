using Padel.Application.Models;
using Padel.Application.Rules;
using Match = Padel.Application.Models.Match;



namespace Padel.Tests
{
    public class GenderBalanceRuleTests
    {

        [Fact]
        public void Validate_Returns0_WhenTeamsHaveEqualGenderBalance()
        {
            // Arrange
            var male1 = new Participant { Id = Guid.NewGuid(), Gender = "M" };
            var female1 = new Participant { Id = Guid.NewGuid(), Gender = "F" };
            var male2 = new Participant { Id = Guid.NewGuid(), Gender = "M" };
            var female2 = new Participant { Id = Guid.NewGuid(), Gender = "F" };

            var team1 = new Team(male1, female1);
            var team2 = new Team(male2, female2);

            var match = new Match(team1, team2);
            var rule = new GenderBalanceRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            // Assert
            Assert.Equal(0, result);  // Expecting 0% fault as the gender balance is equal
        }

        [Fact]
        public void Validate_Returns100_When_2Males_VS_2Females()
        {
            // Arrange
            var male1 = new Participant { Id = Guid.NewGuid(), Gender = "M" };
            var male2 = new Participant { Id = Guid.NewGuid(), Gender = "M" };
            var female1 = new Participant { Id = Guid.NewGuid(), Gender = "F" };
            var female2 = new Participant { Id = Guid.NewGuid(), Gender = "F" };

            var team1 = new Team(male1, male2);
            var team2 = new Team(female1, female2);

            var match = new Match(team1, team2);
            var rule = new GenderBalanceRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            // Assert
            Assert.Equal(100, result);  // Expecting 100% fault as the gender balance is equal
        }

        [Fact]
        public void Validate_Returns0_When_2Males_VS_2Males()
        {
            // Arrange
            var male1 = new Participant { Id = Guid.NewGuid(), Gender = "M" };
            var male2 = new Participant { Id = Guid.NewGuid(), Gender = "M" };
            var male3 = new Participant { Id = Guid.NewGuid(), Gender = "M" };
            var male4 = new Participant { Id = Guid.NewGuid(), Gender = "M" };

            var team1 = new Team(male1, male2);
            var team2 = new Team(male3, male4);

            var match = new Match(team1, team2);
            var rule = new GenderBalanceRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            // Assert
            Assert.Equal(0, result);  // Expecting 100% fault as the gender balance is equal
        }


        [Fact]
        public void Validate_Returns0_When_2Females_VS_2Females()
        {
            // Arrange
            var female1 = new Participant { Id = Guid.NewGuid(), Gender = "F" };
            var female2 = new Participant { Id = Guid.NewGuid(), Gender = "F" };
            var female3 = new Participant { Id = Guid.NewGuid(), Gender = "F" };
            var female4= new Participant { Id = Guid.NewGuid(), Gender = "F" };

            var team1 = new Team(female1, female2);
            var team2 = new Team(female3, female4);

            var match = new Match(team1, team2);
            var rule = new GenderBalanceRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            // Asser
            Assert.Equal(0, result);

        }

        [Fact]
        public void Validate_Returns100_WhenTeamsHaveUnequalGenderBalance()
        {
            var female1 = new Participant {Id = Guid.NewGuid(), Gender = "F"};
            var female2 = new Participant { Id = Guid.NewGuid(), Gender = "F" };
            var female3 = new Participant { Id = Guid.NewGuid(), Gender = "F" };
            var male1 = new Participant { Id = Guid.NewGuid(), Gender = "M" };

            var team1 = new Team(female1, female2);
            var team2 = new Team(female3, male1);

            var match = new Match(team1, team2);
            var rule = new GenderBalanceRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            // Assert
            Assert.Equal(100, result);
        }





    }
}