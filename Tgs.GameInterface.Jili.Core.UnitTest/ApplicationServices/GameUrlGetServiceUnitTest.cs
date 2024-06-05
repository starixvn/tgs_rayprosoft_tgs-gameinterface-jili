using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Core.Models;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
	[Fact]
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
			IsPlayerExist: false,
			GameUrl: string.Empty);

		_ = fakeClient.LoginWithoutRedirectAsync(
			apiUrl: Arg.Is(apiUrl),
			agentId: Arg.Is(agentId),
			agentKey: Arg.Is(agentKey),
			playerCode: Arg.Is(playerCode),
			gameId: Arg.Is(externalGameId),
			lang: Arg.Is(cultureCode)).Returns(result);

		var service = new GameUrlGetService(fakeClient);

		var actual = await service.GetAsync(query);

		Assert.False(actual.IsPlayerExist);
		Assert.Empty(actual.GameUrl);
	}

	[Fact]
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
			IsPlayerExist: false,
			GameUrl: string.Empty);

		_ = fakeClient.LoginWithoutRedirectAsync(
			apiUrl: Arg.Is(apiUrl),
			agentId: Arg.Is(agentId),
			agentKey: Arg.Is(agentKey),
			playerCode: Arg.Is(playerCode),
			gameId: Arg.Is(externalGameId),
			lang: Arg.Is(cultureCode)).Throws<Exception>();

		var service = new GameUrlGetService(fakeClient);

		_ = await Assert.ThrowsAsync<Exception>(() => service.GetAsync(query).AsTask());
	}

	[Fact]
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

		var actual = await service.GetAsync(query);

		Assert.True(actual.IsPlayerExist);
		Assert.NotEmpty(actual.GameUrl);
	}
}
