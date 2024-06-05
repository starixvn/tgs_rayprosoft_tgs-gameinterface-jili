using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.IntegrationTest;

public class PlayerBalanceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://uat-wb-api.jlfafafa2.com/");
		var agentId = "PG_GPI_CNY";
		var agentKey = "82af5971ae233eaa003b774be4079e7902ea7ff4";
		var playerCode = "4ucmqplxb029";

		var web = new GameInterfaceJiliApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<PlayerBalanceQuery, PlayerBalanceResult>>();

			var query = new PlayerBalanceQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			PlayerCode: playerCode);

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new PlayerBalanceResult(
				IsPlayerExist: true,
				Balance: 100m,
				Online: true));

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceJiliClient(new Protos.GameInterfaceJili.GameInterfaceJiliClient(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.PlayerBalanceAsync(
			apiUrl: apiUrl,
			agentId: agentId,
			agentKey: agentKey,
			playerCode: playerCode);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(100m, actual.Balance.ToDecimal().GetValueOrDefault(0.0m));
	}
}
