using System.Text.Json;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.IntegrationTest;

public class FundStatusUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://uat-wb-api.jlfafafa2.com/");
		var agentId = "PG_GPI_CNY";
		var agentKey = "82af5971ae233eaa003b774be4079e7902ea7ff4";
		var transactionId = Guid.NewGuid().ToString("N");

		var web = new GameInterfaceJiliApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<FundStatusQuery, bool>>();

			var query = new FundStatusQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			TransactionId: transactionId);

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(true);

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceJiliClient(new Protos.GameInterfaceJili.GameInterfaceJiliClient(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.FundStatusAsync(
			apiUrl: apiUrl,
			agentId: agentId,
			agentKey: agentKey,
			transactionId: transactionId);

		Assert.True(actual);
	}
}
