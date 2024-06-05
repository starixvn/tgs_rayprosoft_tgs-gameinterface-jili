using System.Text.Json;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.IntegrationTest;

public class FundOutUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://uat-wb-api.jlfafafa2.com/");
		var agentId = "PG_GPI_CNY";
		var agentKey = "82af5971ae233eaa003b774be4079e7902ea7ff4";
		var playerCode = "4ucmqplxb029";
		var transactionId = Guid.NewGuid().ToString("N");
		var amount = 100.0m;

		var web = new GameInterfaceJiliApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<FundOutQuery, FundOutResult>>();

			var query = new FundOutQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			PlayerCode: playerCode,
			TransactionId: transactionId,
			Amount: amount);

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new FundOutResult(
				TransactionId: transactionId,
				CoinBefore: 0.0m,
				CoinAfter: 100.0m,
				CurrencyAfter: 0.0m,
				CurrencyBefore: 100.0m,
				Resp: JsonSerializer.Serialize(new
				{
					TransactionId = transactionId,
					CoinBefore = 0.0m,
					CoinAfter = 100.0m,
					CurrencyBefore = 0.0m,
					CurrencyAfter = 100.0m,
					Status = 1
				})));

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceJiliClient(new Protos.GameInterfaceJili.GameInterfaceJiliClient(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.FundOutAsync(
			apiUrl: apiUrl,
			agentId: agentId,
			agentKey: agentKey,
			playerCode: playerCode,
			transactionId: transactionId,
			amount: amount);

		Assert.NotNull(actual);
		Assert.Equal(transactionId, actual.TransactionId);
	}
}
