using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.JiliService.Core;
using Tgs.Logging;

namespace Tgs.GameInterface.JiliService.Repositories.UnitTest;

[TestClass]
public class ExchangeTransferByAgentIdUnitTest
{
    [TestMethod]
    public async Task Normal_Process_FundIn()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";
        var transactionId = Guid.NewGuid().ToString("N");
        var transferType = TransferType.FundIn;
        var amount = 10m;
        var response = new
        {
            ErrorCode = "0",
            Message = string.Empty,
            Data = new
            {
                TransactionId = transactionId,
                CoinBefore = 10m,
                CoinAfter = 0m,
                CurrencyBefore = 10m,
                CurrencyAfter = 10m,
                Status = 1
            }
        };

        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/api1/ExchangeTransferByAgentId")
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeTgsLog = Substitute.For<ITgsLog>();

        var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

        var atual = await client.ExchangeTransferByAgentIdAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              playerCode: playerCode,
              transactionId: transactionId,
              transferType: transferType,
              amount: amount).ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(true, atual.Success);
        Assert.AreEqual(transactionId, atual.TransactionId);
    }

    [TestMethod]
    public async Task Normal_Process_FundOut()
    {
        var apiUrl = new Uri("http://localhost/");
        var agentId = "testAgentId";
        var agentKey = "testAgentKey";
        var playerCode = "test";
        var transactionId = Guid.NewGuid().ToString("N");
        var transferType = TransferType.FundOut;
        var amount = 10m;
        var response = new
        {
            ErrorCode = "0",
            Message = string.Empty,
            Data = new
            {
                TransactionId = transactionId,
                CoinBefore = 0m,
                CoinAfter = 10m,
                CurrencyBefore = 10m,
                CurrencyAfter = 10m,
                Status = 1
            }
        };

        var content = JsonSerializer.Serialize(response);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/api1/ExchangeTransferByAgentId")
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeTgsLog = Substitute.For<ITgsLog>();

        var client = new GameApiClient(fakeHttpClient, fakeLog, fakeTgsLog);

        var atual = await client.ExchangeTransferByAgentIdAsync(
              apiUrl: apiUrl,
              agentId: agentId,
              agentKey: agentKey,
              playerCode: playerCode,
              transactionId: transactionId,
              transferType: transferType,
              amount: amount).ConfigureAwait(false);

        Assert.IsNotNull(atual);
        Assert.AreEqual(true, atual.Success);
        Assert.AreEqual(transactionId, atual.TransactionId);
    }
}
