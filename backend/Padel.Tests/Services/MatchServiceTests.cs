using Moq;
using Padel.Application.Services;

namespace Padel.Tests.Services
{
    public class MatchServiceTests
    {
        private readonly Mock<ITeamService> _matchServiceMock;
        public MatchServiceTests()
        {
            _matchServiceMock = new Mock<ITeamService>();
        }

       



    }

}
