using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.JiliService.Core.UnitTest;

[TestClass]
public class GetBetRecordByTimeUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var startTime = DateTime.UtcNow.AddMinutes(-5);
        var endTime = DateTime.UtcNow;

        var response = new
        {
            ErrorCode = 0,
            Message = "",
            Data = new
            {
                Result = new[]{
                   new{ Account="4uiuj5ovw7zk",
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
                    Turnover=1m}
                },
                Pagination = new
                {
                    CurrentPage = 1,
                    TotalPages = 1,
                    PageLimit = 1000,
                    TotalNumber = 1
                }
            }
        };

        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.When(HttpMethod.Get, "/api1/GetBetRecordByTime")
                    .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var atual = await client.GetBetRecordByTimeAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              startTime: startTime,
              endTime: endTime)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(atual.Single().BetAmount, 1);
        Assert.AreEqual(atual.Single().Turnover, 1);
        Assert.AreEqual(atual.Single().AdditionalData, JsonSerializer.Serialize(response.Data.Result.Single()));
    }

    [TestMethod]
    public async Task Normal_Process_PG_PB2_PHP_32()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var startTime = DateTimeOffset.Parse("2023-03-14T12:00:00+08:00").UtcDateTime;
        var endTime = DateTimeOffset.Parse("2023-03-14T13:00:00+08:00").UtcDateTime;

        var content = "{\"ErrorCode\":0,\"Message\":\"\",\"Data\":{\"Result\":[{\"Account\":\"4ui5iyakdk9m\",\"WagersId\":25869908520032098,\"GameId\":32,\"WagersTime\":\"2023-03-14T00:54:54-04:00\",\"BetAmount\":-296.4,\"PayoffTime\":\"2023-03-14T00:54:54-04:00\",\"PayoffAmount\":388.5,\"Status\":1,\"SettlementTime\":\"2023-03-14T00:54:54-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":296.4},{\"Account\":\"4ui5iyakdk9m\",\"WagersId\":25869867320032823,\"GameId\":32,\"WagersTime\":\"2023-03-14T00:50:43-04:00\",\"BetAmount\":-654.6,\"PayoffTime\":\"2023-03-14T00:50:43-04:00\",\"PayoffAmount\":711,\"Status\":1,\"SettlementTime\":\"2023-03-14T00:50:43-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":654.6},{\"Account\":\"4ui5iyakdk9m\",\"WagersId\":25869831420032877,\"GameId\":32,\"WagersTime\":\"2023-03-14T00:42:35-04:00\",\"BetAmount\":-335.4,\"PayoffTime\":\"2023-03-14T00:42:35-04:00\",\"PayoffAmount\":294,\"Status\":2,\"SettlementTime\":\"2023-03-14T00:42:35-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":335.4},{\"Account\":\"4ui5iyakdk9m\",\"WagersId\":25869716420032721,\"GameId\":32,\"WagersTime\":\"2023-03-14T00:38:16-04:00\",\"BetAmount\":-1800,\"PayoffTime\":\"2023-03-14T00:38:16-04:00\",\"PayoffAmount\":2282.4,\"Status\":1,\"SettlementTime\":\"2023-03-14T00:38:16-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":1800},{\"Account\":\"4ui5iyakdk9m\",\"WagersId\":25869692520032041,\"GameId\":32,\"WagersTime\":\"2023-03-14T00:18:54-04:00\",\"BetAmount\":-39.4,\"PayoffTime\":\"2023-03-14T00:18:54-04:00\",\"PayoffAmount\":60.9,\"Status\":1,\"SettlementTime\":\"2023-03-14T00:18:54-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":39.4},{\"Account\":\"4ui5iyakdk9m\",\"WagersId\":25869692520032041,\"GameId\":32,\"WagersTime\":\"2023-03-14T00:18:54-04:00\",\"BetAmount\":-39.4,\"PayoffTime\":\"2023-03-14T00:18:54-04:00\",\"PayoffAmount\":60.9,\"Status\":1,\"SettlementTime\":\"2023-03-14T00:18:54-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":39.4}],\"Pagination\":{\"CurrentPage\":1,\"TotalPages\":1,\"PageLimit\":20000,\"TotalNumber\":6}}}";
        var handler = new MockHttpMessageHandler();
        _ = handler.When(HttpMethod.Get, "/api1/GetBetRecordByTime")
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var atual = await client.GetBetRecordByTimeAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              startTime: startTime,
              endTime: endTime,
              gameId: 32,
              pageLimit: 20000)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(atual);
    }

    [TestMethod]
    public async Task Normal_Process_PG_PB2_PHP_1()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var startTime = DateTimeOffset.Parse("2023-03-14T12:00:00+08:00").UtcDateTime;
        var endTime = DateTimeOffset.Parse("2023-03-14T13:00:00+08:00").UtcDateTime;

        var content = "{\"ErrorCode\":0,\"Message\":\"\",\"Data\":{\"Result\":[{\"Account\":\"4uic2v7iebir\",\"WagersId\":25869739121001081,\"GameId\":1,\"WagersTime\":\"2023-03-14T00:43:15-04:00\",\"BetAmount\":-7141,\"PayoffTime\":\"2023-03-14T00:43:15-04:00\",\"PayoffAmount\":5890,\"Status\":2,\"SettlementTime\":\"2023-03-14T00:43:15-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":7141},{\"Account\":\"4uic2v7iebir\",\"WagersId\":25869432721001275,\"GameId\":1,\"WagersTime\":\"2023-03-14T00:19:16-04:00\",\"BetAmount\":-23866,\"PayoffTime\":\"2023-03-14T00:19:16-04:00\",\"PayoffAmount\":24215,\"Status\":1,\"SettlementTime\":\"2023-03-14T00:19:16-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":23866},{\"Account\":\"4uic2v7iebir\",\"WagersId\":25869432721001275,\"GameId\":1,\"WagersTime\":\"2023-03-14T00:19:16-04:00\",\"BetAmount\":-23866,\"PayoffTime\":\"2023-03-14T00:19:16-04:00\",\"PayoffAmount\":24215,\"Status\":1,\"SettlementTime\":\"2023-03-14T00:19:16-04:00\",\"GameCategoryId\":5,\"VersionKey\":0,\"Type\":1,\"AgentId\":\"PG_PB2_PHP\",\"Turnover\":23866}],\"Pagination\":{\"CurrentPage\":1,\"TotalPages\":1,\"PageLimit\":20000,\"TotalNumber\":3}}}";
        var handler = new MockHttpMessageHandler();
        _ = handler.When(HttpMethod.Get, "/api1/GetBetRecordByTime")
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var atual = await client.GetBetRecordByTimeAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              startTime: startTime,
              endTime: endTime,
              gameId: 1,
              pageLimit: 20000)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(atual);
    }
}
