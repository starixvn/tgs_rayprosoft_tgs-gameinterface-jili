using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tgs.GameInterface.JiliService.Core.ApplicationServices;
using Tgs.GameInterface.JiliService.Core.Models;

namespace Tgs.GameInterface.JiliService.Core.UnitTest;

[TestClass]
public class GameUrlGetServiceUnitTest
{
    [TestMethod]
    public async Task Account_Is_Not_Exists()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";
        var externalGameId = "16";
        var cultureCode = "zh-CN";

        var query = new GameUrlQuery(
            ApiUrl: apiUrl,
            AgentId: agentId,
            AgentKey: agentKey,
            PlayerCode: playerCode,
            ExternalGameId: externalGameId,
            Language: cultureCode);

        var fakeClient = Substitute.For<IGameApiClient>();

        var result = new LoginWithoutRedirectResult(
            Success: true,
            IsPlayerExist: false,
            GameUrl: null);

        _ = fakeClient.LoginWithoutRedirectAsync(
            apiUrl: Arg.Is(apiUrl),
            agentId: Arg.Is(agentId),
            agentKey: Arg.Is(agentKey),
            playerCode: Arg.Is(playerCode),
            gameId: Arg.Is(externalGameId),
            lang: Arg.Is(cultureCode)).Returns(result);

        var service = new GameUrlGetService(fakeClient);

        var actual = await service.GetAsync(query).ConfigureAwait(false);

        Assert.IsTrue(actual.Success);
        Assert.IsFalse(actual.IsPlayerExist);
        Assert.IsNull(actual.GameUrl);
    }

    [TestMethod]
    public async Task Exception_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";
        var externalGameId = "16";
        var cultureCode = "zh-CN";

        var query = new GameUrlQuery(
            ApiUrl: apiUrl,
            AgentId: agentId,
            AgentKey: agentKey,
            PlayerCode: playerCode,
            ExternalGameId: externalGameId,
            Language: cultureCode);

        var fakeClient = Substitute.For<IGameApiClient>();

        var result = new LoginWithoutRedirectResult(
            Success: true,
            IsPlayerExist: false,
            GameUrl: null);

        _ = fakeClient.LoginWithoutRedirectAsync(
            apiUrl: Arg.Is(apiUrl),
            agentId: Arg.Is(agentId),
            agentKey: Arg.Is(agentKey),
            playerCode: Arg.Is(playerCode),
            gameId: Arg.Is(externalGameId),
            lang: Arg.Is(cultureCode)).Throws<Exception>();

        var service = new GameUrlGetService(fakeClient);

        _ = await Assert.ThrowsExceptionAsync<Exception>(() => service.GetAsync(query).AsTask()).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";
        var externalGameId = "16";
        var cultureCode = "zh-CN";

        var query = new GameUrlQuery(
            ApiUrl: apiUrl,
            AgentId: agentId,
            AgentKey: agentKey,
            PlayerCode: playerCode,
            ExternalGameId: externalGameId,
            Language: cultureCode);

        var fakeClient = Substitute.For<IGameApiClient>();

        var result = new LoginWithoutRedirectResult(
            Success: true,
            IsPlayerExist: true,
            GameUrl: "https://www.google.com");

        _ = fakeClient.LoginWithoutRedirectAsync(
            apiUrl: Arg.Is(apiUrl),
            agentId: Arg.Is(agentId),
            agentKey: Arg.Is(agentKey),
            playerCode: Arg.Is(playerCode),
            gameId: Arg.Is(externalGameId),
            lang: Arg.Is(cultureCode)).Returns(result);

        var service = new GameUrlGetService(fakeClient);

        var actual = await service.GetAsync(query).ConfigureAwait(false);

        Assert.IsTrue(actual.Success);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.IsNotNull(actual.GameUrl);
    }
}
