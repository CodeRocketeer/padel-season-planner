using Xunit;
using Moq;
using System.Linq;
using System.Collections.Generic;
using Padel.Application.Models;
using Padel.Application.Rules;
using Match = Padel.Application.Models.Match;



namespace Padel.Tests
{
    public class CommonPlayerRuleTests
    {

        [Fact]
        public void Validate_Returns100_WhenParticipantsAreOnBothTeams()
        {
            // Arrange
            var participant1 = new Participant { Id = Guid.NewGuid() };
            var participant2 = new Participant { Id = Guid.NewGuid() };
            var sameParticipant = new Participant { Id = Guid.NewGuid() };

            // Create two teams that share the same participant
            var team1 = new Team(participant1, sameParticipant);
            var team2 = new Team(participant2, sameParticipant);

            // Create a match with the two teams
            var match = new Match(team1, team2);

            // Create the rule instance to test
            var rule = new CommonPlayerRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            // Assert
            Assert.Equal(100, result);  // Expecting a 100% fault as the same participant is on both teams
        }

        [Fact]
        public void Validate_Returns0_WhenNoParticipantsAreOnBothTeams()
        {
            // Arrange
            var participant1 = new Participant { Id = Guid.NewGuid() };
            var participant2 = new Participant { Id = Guid.NewGuid() };
            var participant3 = new Participant { Id = Guid.NewGuid() };
            var participant4 = new Participant { Id = Guid.NewGuid() };

            // Create two teams that share the same participant
            var team1 = new Team(participant1, participant2);
            var team2 = new Team(participant3, participant4);

            // Create a match with the two teams
            var match = new Match(team1, team2);

            // Create the rule instance to test
            var rule = new CommonPlayerRule();

            // Act
            var result = rule.Validate(match, new List<Match>(), new List<Participant>());

            // Assert
            Assert.Equal(0, result);  // Expecting a 0% fault as no participants are on both teams
        }



    }
}