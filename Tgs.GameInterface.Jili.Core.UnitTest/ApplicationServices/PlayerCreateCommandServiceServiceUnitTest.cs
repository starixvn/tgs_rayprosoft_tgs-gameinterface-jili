using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Tgs.GameInterface.Jili.Core.ApplicationServices;

namespace Tgs.GameInterface.Jili.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceServiceUnitTest
{
	[Fact]
	public async Task Failed_To_Create_Member()
	{
		var apiUrl = new Uri("http://localhost/");
		var agentId = "testAgentId";
		var agentKey = "testAgentKey";
		var playerCode = "test";

		var query = new PlayerCreateCommand(
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

		var service = new PlayerCreateCommandService(fakeClient);

		_ = await Assert.ThrowsAsync<Exception>(() => service.ExecuteAsync(query).AsTask());
	}

	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://localhost/");
		var agentId = "testAgentId";
		var agentKey = "testAgentKey";
		var playerCode = "test";

		var query = new PlayerCreateCommand(
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

		var service = new PlayerCreateCommandService(fakeClient);

		await service.ExecuteAsync(query);
	}
}
