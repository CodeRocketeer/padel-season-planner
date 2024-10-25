using FluentAssertions;
using Padel.Application.Models;

namespace Padel.Tests.Models;

public class PlayerTests
{


    [Fact]
    public void CreateNewParticipant_Creates()
    {
        var participant = new Player(Gender.Male ,"Joske");
        
        participant.Name.Should().Be("Joske");
        participant.Gender.Should().Be(Gender.Male);
    }

   
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateNewParticipant_Name_Empty_Throws(string name)
    {

        FluentActions.Invoking(() => new Player(Gender.Female, name)).Should().Throw<ArgumentNullException>();
    }
}