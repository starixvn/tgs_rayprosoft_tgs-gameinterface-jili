using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.Logging;

namespace Tgs.GameInterface.JiliService.Repositories.UnitTest;

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
            ErrorCode = "0",
            Message = string.Empty,
            Data = new
            {
                Result = new[] {
                    new
                    {
                        Account = "4ucmqplxb029",
                        WagersId = 1655343469000372013,
                        GameId = 13,
                        WagersTime = "2022-06-16T10:03:07-04:00",
                        BetAmount = -3,
                        PayoffTime = "2022-06-16T10:03:07-04:00",
                        PayoffAmount = 0,
                        Status = 2,
                        SettlementTime = "2022-06-16T10:03:07-04:00",
                        GameCategoryId = 1,
                        VersionKey = 0,
                        Type = 1,
                        AgentId = "PG_GPI_CNY"
                    }
                },
                Pagination = new
                {
                    CurrentPage = 1,
                    TotalPages = 1,
                    PageLimit = 100,
                    TotalNumber = 1
                }
            }
        };

        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.When(HttpMethod.Get, "/api1/GetBetRecordByTime")
            .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeTgsLog = Substitute.For<ITgsLog>();

        var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

        var atual = await client.GetBetRecordByTimeAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              startTime: startTime,
              endTime: endTime).ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(true, atual.Success);
        Assert.IsNotNull(atual.Transactions);
        Assert.IsTrue(atual.Transactions.Single().BetAmount > 0);
    }
}
