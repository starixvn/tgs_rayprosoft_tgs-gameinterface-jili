using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.IntegrationTest;

public class PlayerCreateUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://uat-wb-api.jlfafafa2.com/");
		var agentId = "PG_GPI_CNY";
		var agentKey = "82af5971ae233eaa003b774be4079e7902ea7ff4";
		var playerCode = "4ucmqplxb029";

		var fakeService = Substitute.For<ICommandService<PlayerCreateCommand>>();

		var web = new GameInterfaceJiliApplication(builder => _ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeService)));

		var sut = new Client.GameInterfaceJiliClient(new Protos.GameInterfaceJili.GameInterfaceJiliClient(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		await sut.PlayerCreateAsync(
		  apiUrl: apiUrl,
		  agentId: agentId,
		  agentKey: agentKey,
		  playerCode: playerCode);

		_ = fakeService
			.Received(1)
			.ExecuteAsync(
			command: Arg.Is(new PlayerCreateCommand(
				ApiUrl: apiUrl,
				AgentId: agentId,
				AgentKey: agentKey,
				PlayerCode: playerCode)),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
