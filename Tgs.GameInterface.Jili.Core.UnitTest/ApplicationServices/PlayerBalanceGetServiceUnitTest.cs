using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Core.Models;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Account_Is_Not_Exists()
	{
		var apiUrl = new Uri("http://localhost/");
		var agentId = "testAgentId";
		var agentKey = "testAgentKey";
		var playerCode = "test";

		var query = new PlayerBalanceQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			PlayerCode: playerCode);

		var fakeClient = Substitute.For<IGameApiClient>();

		var result = new GetMemberInfoResult(
			IsPlayerExist: false,
			Balance: 0.0m,
			Online: false);
		_ = fakeClient.GetMemberInfoAsync(
			Arg.Is(apiUrl),
			Arg.Is(agentId),
			Arg.Is(agentKey),
			Arg.Is(playerCode)).Returns(ValueTask.FromResult(result));

		var service = new PlayerBalanceGetService(fakeClient);

		var actual = await service.GetAsync(query);

		Assert.False(actual.IsPlayerExist);
		Assert.Equal(0.0m, actual.Balance);
		Assert.False(actual.Online);
	}

	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var agentId = "testAgentId";
		var agentKey = "testAgentKey";
		var playerCode = "test";

		var query = new PlayerBalanceQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			PlayerCode: playerCode);

		var fakeClient = Substitute.For<IGameApiClient>();

		var result = new GetMemberInfoResult(
			IsPlayerExist: true,
			Balance: 100.0m,
			Online: true);
		_ = fakeClient.GetMemberInfoAsync(
			Arg.Is(apiUrl),
			Arg.Is(agentId),
			Arg.Is(agentKey),
			Arg.Is(playerCode)).Returns(ValueTask.FromResult(result));

		var service = new PlayerBalanceGetService(fakeClient);

		var actual = await service.GetAsync(query);

		Assert.True(actual.IsPlayerExist);
		Assert.Equal(100.0m, actual.Balance);
		Assert.True(actual.Online);
	}
}
