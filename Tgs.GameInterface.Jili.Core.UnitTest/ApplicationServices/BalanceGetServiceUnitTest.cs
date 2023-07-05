using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Core.Models;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

[TestClass]
public class BalanceGetServiceUnitTest
{
    [TestMethod]
    public async Task Account_Is_Not_Exists()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";

        var query = new BalanceQuery(
            ApiUrl: apiUrl,
            AgentId: agentId,
            AgentKey: agentKey,
            PlayerCode: playerCode);

        var fakeClient = Substitute.For<IGameApiClient>();

        var result = new GetMemberInfoResult(
            Success: true,
            IsPlayerExist: false,
            Balance: null);
        _ = fakeClient.GetMemberInfoAsync(
            Arg.Is(apiUrl),
            Arg.Is(agentId),
            Arg.Is(agentKey),
            Arg.Is(playerCode)).Returns(ValueTask.FromResult(result));

        var service = new BalanceGetService(fakeClient);

        var actual = await service.GetAsync(query).ConfigureAwait(false);

        Assert.IsTrue(actual.Success);
        Assert.IsFalse(actual.IsPlayerExist);
        Assert.IsNull(actual.Balance);
    }

    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";

        var query = new BalanceQuery(
            ApiUrl: apiUrl,
            AgentId: agentId,
            AgentKey: agentKey,
            PlayerCode: playerCode);

        var fakeClient = Substitute.For<IGameApiClient>();

        var result = new GetMemberInfoResult(
            Success: true,
            IsPlayerExist: true,
            Balance: 100m);
        _ = fakeClient.GetMemberInfoAsync(
            Arg.Is(apiUrl),
            Arg.Is(agentId),
            Arg.Is(agentKey),
            Arg.Is(playerCode)).Returns(ValueTask.FromResult(result));

        var service = new BalanceGetService(fakeClient);

        var actual = await service.GetAsync(query).ConfigureAwait(false);

        Assert.IsTrue(actual.Success);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.AreEqual(result.Balance, actual.Balance);
    }
}
