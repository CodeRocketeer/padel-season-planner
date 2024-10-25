using FluentAssertions;
using Padel.Application.Models;

namespace Padel.Tests.Models;

public class TeamTests
{

    [Fact]
    public void Team_With_Same_Player_throws()
    {
        var p1 = new Player(1, Gender.Female, "Joske");

        FluentActions.Invoking(() => new Team(p1, p1)).Should().Throw<Exception>();
    }

}