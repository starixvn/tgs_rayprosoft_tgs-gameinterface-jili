using System.Security.Principal;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tgs.GameInterface.Jili.Core.ApplicationServices;
using Tgs.GameInterface.Jili.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Jili.IntegrationTest;

public class GameRecordUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://uat-wb-api.jlfafafa2.com/");
		var agentId = "PG_GPI_CNY";
		var agentKey = "82af5971ae233eaa003b774be4079e7902ea7ff4";
		var startTime = DateTime.UtcNow.AddMinutes(-5);
		var endTime = DateTime.UtcNow;
		var page = 1;
		var pageLimit = 5000;

		var web = new GameInterfaceJiliApplication(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IQueryService<GameRecordQuery, GameRecordResult>>();

			var query = new GameRecordQuery(
			ApiUrl: apiUrl,
			AgentId: agentId,
			AgentKey: agentKey,
			StartTime: startTime,
			EndTime: endTime,
			Page: page,
			PageLimit: pageLimit);

			_ = fakeService.QueryAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new GameRecordResult[] {
				new(
					Account:"4uiuj5ovw7zk",
					WagersId:"1680840597057173115",
					GameId:115,
					WagersTime:DateTimeOffset.Parse( "2023-04-17T21:40:53-04:00"),
					BetAmount:-1m,
					PayoffTime:DateTimeOffset.Parse("2023-04-17T21:40:53-04:00"),
					PayoffAmount:0.8m,
					Status:(Core.JiLiStatus)2,
					SettlementTime:DateTimeOffset.Parse("2023-04-17T21:40:53-04:00"),
					GameCategoryId:1,
					Type:1,
					AgentId:"PG_GPI_CNY",
					Turnover:1m,
					AdditionalData:System.Text.Json.JsonSerializer.Serialize(new{
						Account="4uiuj5ovw7zk",
						WagersId=1680840597057173115,
						GameId=115,
						WagersTime="2023-04-17T21:40:53-04:00",
						BetAmount=-1m,
						PayoffTime="2023-04-17T21:40:53-04:00",
						PayoffAmount=0.8m,
						Status=2,
						SettlementTime="2023-04-17T21:40:53-04:00",
						GameCategoryId=1,
						VersionKey=0,
						Type=1,
						AgentId="PG_GPI_CNY",
						Turnover=1m}))
			}.ToAsyncEnumerable());

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceJiliClient(new Protos.GameInterfaceJili.GameInterfaceJiliClient(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.GameRecordAsync(
			apiUrl: apiUrl,
			agentId: agentId,
			agentKey: agentKey,
			startTime: startTime,
			endTime: endTime,
			page: page,
			pageLimit: pageLimit)
			.ToArrayAsync()
			?? Enumerable.Empty<GameRecordInfo>();

		Assert.NotNull(actual);
		Assert.Equal("4uiuj5ovw7zk", actual.First().Account);
		Assert.Equal("1680840597057173115", actual.First().WagersId);
		Assert.Equal(115, actual.First().GameId);
		Assert.Equal(DateTimeOffset.Parse("2023-04-17T21:40:53-04:00"), actual.First().WagersTime.ToDateTimeOffset());
		Assert.Equal(DateTimeOffset.Parse("2023-04-17T21:40:53-04:00"), actual.First().PayoffTime.ToDateTimeOffset());
		Assert.Equal(DateTimeOffset.Parse("2023-04-17T21:40:53-04:00"), actual.First().SettlementTime.ToDateTimeOffset());
		Assert.Equal(GameRecordStatus.Lose, actual.First().Status);
		Assert.Equal(-1m, actual.First().BetAmount.ToDecimal().GetValueOrDefault(0.0m));
		Assert.Equal(1m, actual.First().Turnover.ToDecimal().GetValueOrDefault(0.0m));
		Assert.Equal(0.8m, actual.First().PayoffAmount.ToDecimal().GetValueOrDefault(0.0m));
		Assert.NotEmpty(actual.First().AdditionalData);
	}
}
