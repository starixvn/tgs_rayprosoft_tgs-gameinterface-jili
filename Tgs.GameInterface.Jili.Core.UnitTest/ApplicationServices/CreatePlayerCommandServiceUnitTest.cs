using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tgs.GameInterface.Jili.Core.ApplicationServices;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

[TestClass]
public class CreatePlayerCommandServiceUnitTest
{
    [TestMethod]
    public async Task Failed_To_Create_Member()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";

        var query = new CreatePlayerCommand(
            ApiUrl: apiUrl,
            AgentId: agentId,
            AgentKey: agentKey,
            PlayerCode: playerCode);

        var fakeClient = Substitute.For<IGameApiClient>();

        _ = fakeClient.CreateMemberAsync(
            apiUrl: Arg.Is(apiUrl),
            agentId: Arg.Is(agentId),
            agentKey: Arg.Is(agentKey),
            playerCode: Arg.Is(playerCode)).Throws<Exception>();

        var service = new CreatePlayerCommandService(fakeClient);

        _ = await Assert.ThrowsExceptionAsync<Exception>(() => service.ExecuteAsync(query).AsTask()).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";

        var query = new CreatePlayerCommand(
            ApiUrl: apiUrl,
            AgentId: agentId,
            AgentKey: agentKey,
            PlayerCode: playerCode);

        var fakeClient = Substitute.For<IGameApiClient>();

        _ = fakeClient.CreateMemberAsync(
            apiUrl: Arg.Is(apiUrl),
            agentId: Arg.Is(agentId),
            agentKey: Arg.Is(agentKey),
            playerCode: Arg.Is(playerCode));

        var service = new CreatePlayerCommandService(fakeClient);

        await service.ExecuteAsync(query).ConfigureAwait(false);
    }
}
