using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Core.Models;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

public class PlayerOnlineGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var agentId = "testAgentId";
		var agentKey = "testAgentKey";
		var playerCode = "test";

		var query = new PlayerOnlineQuery(
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

		var service = new PlayerOnlineGetService(fakeClient);

		var actual = await service.GetAsync(query);

		Assert.True(actual);
	}
}
