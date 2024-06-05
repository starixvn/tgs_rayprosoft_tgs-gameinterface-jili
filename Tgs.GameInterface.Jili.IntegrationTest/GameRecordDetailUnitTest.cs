using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.IntegrationTest;

public class GameRecordDetailUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://uat-wb-api.jlfafafa2.com/");
		var agentId = "PG_GPI_CNY";
		var agentKey = "82af5971ae233eaa003b774be4079e7902ea7ff4";
		var wagersId = "1680840597057173115";
		var language = "zh-CN";

		var web = new GameInterfaceJiliApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<GameRecordDetailQuery, string>>();

			var query = new GameRecordDetailQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			WagersId: wagersId,
			Language: language);

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns("http://localhost/");

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceJiliClient(new Protos.GameInterfaceJili.GameInterfaceJiliClient(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.GameRecordDetailAsync(
			apiUrl: apiUrl,
			agentId: agentId,
			agentKey: agentKey,
			wagersId: wagersId,
			language: language);

		Assert.NotNull(actual);
		Assert.NotEmpty(actual.Url);
	}
}
